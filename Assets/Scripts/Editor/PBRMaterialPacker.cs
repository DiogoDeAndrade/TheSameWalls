// Assets/Editor/PBRMaterialPacker.cs
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

public static class PBRMaterialPacker
{
    // ---------- Context menu ----------
    [MenuItem("Assets/One Room Tools/PBR Material Packer", priority = 2000)]
    private static void PackSelectedMaterials()
    {
        var objs = Selection.objects;
        int done = 0, skipped = 0;

        foreach (var o in objs)
        {
            var path = AssetDatabase.GetAssetPath(o);
            var mat = AssetDatabase.LoadAssetAtPath<Material>(path);
            if (mat == null) { skipped++; continue; }

            if (!TryGetBaseMap(mat, out var baseMap))
            {
                Debug.LogWarning($"[PBR] '{mat.name}': no BaseMap/_MainTex found — skipped.");
                skipped++;
                continue;
            }

            var baseTexPath = AssetDatabase.GetAssetPath(baseMap);
            var folder = Path.GetDirectoryName(baseTexPath)?.Replace("\\", "/");
            var root = GuessRootName(Path.GetFileNameWithoutExtension(baseTexPath));

            // Find companion textures
            Texture2D tMetal = FindTex(folder, root, TexKind.Metallic);
            Texture2D tRough = FindTex(folder, root, TexKind.Roughness) ?? FindTex(folder, root, TexKind.Smoothness);
            Texture2D tRoughOrSmooth = FindTex(folder, root, TexKind.Roughness)
                            ?? FindTex(folder, root, TexKind.Smoothness);
            bool    sourceIsSmoothness = tRoughOrSmooth != null && NameLooksSmoothness(AssetDatabase.GetAssetPath(tRoughOrSmooth));


            Texture2D tHeight = FindTex(folder, root, TexKind.Height);
            Texture2D tAO     = FindTex(folder, root, TexKind.AO);
            Texture2D tEmiss  = FindTex(folder, root, TexKind.Emissive);

            // Pack Metal (R) + Smooth (A)
            Texture2D packed = null;
            if (tMetal != null || tRoughOrSmooth != null)
            {
                bool invertAlpha = (tRoughOrSmooth != null) && !sourceIsSmoothness; // true when source is Roughness
                packed = CreatePackedMetalR_SmoothA(tMetal, tRoughOrSmooth, invertAlpha, folder, root);
                if (packed != null && TryGetMetalMapProp(mat, out string metalProp))
                    mat.SetTexture(metalProp, packed);
            }

            // Assign Height / AO / Emission
            if (tHeight != null && TryGetParallaxProp(mat, out var parProp)) mat.SetTexture(parProp, tHeight);
            if (tAO     != null && TryGetAOProp(mat, out var aoProp))       mat.SetTexture(aoProp, tAO);

            if (tEmiss != null && TryGetEmissionProps(mat, out var emMapProp, out var emColorProp))
            {
                mat.EnableKeyword("_EMISSION");
                if (!mat.HasColor(emColorProp)) mat.SetColor(emColorProp, Color.white);
                else if (mat.GetColor(emColorProp).maxColorComponent <= 0f) mat.SetColor(emColorProp, Color.white);
                mat.SetTexture(emMapProp, tEmiss);
            }

            // sRGB OFF for all non-color textures
            ForceSRGB(baseMap, true);           // base color stays sRGB ON
            ForceSRGB(packed, false);
            ForceSRGB(tMetal, false);
            ForceSRGB(tRough, false);
            ForceSRGB(tHeight, false);
            ForceSRGB(tAO, false);
            ForceSRGB(tEmiss, true);    // emissive color stays sRGB ON

            EditorUtility.SetDirty(mat);
            done++;
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log($"[PBR] Processed {done} material(s), skipped {skipped}.");
    }

    [MenuItem("Assets/One Room Tools/PBR Material Packer", true)]
    private static bool PackSelectedMaterials_Validate()
        => Selection.objects.Any(o => AssetDatabase.GetMainAssetTypeAtPath(AssetDatabase.GetAssetPath(o)) == typeof(Material));

    // ---------- Packing ----------
    private static Texture2D CreatePackedMetalR_SmoothA(Texture2D metal, Texture2D roughOrSmooth, bool invertAlpha, string folder, string root)
    {
        // choose size
        int w = 0, h = 0;
        if (metal != null) { w = metal.width; h = metal.height; }
        if (w == 0 && roughOrSmooth != null) { w = roughOrSmooth.width; h = roughOrSmooth.height; }
        if (w == 0 || h == 0) return null;

        var m = EnsureReadable(metal, w, h);
        var rs = EnsureReadable(roughOrSmooth, w, h);

        var outTex = new Texture2D(w, h, TextureFormat.RGBA32, true, true);
        var mPixels = m != null ? m.GetPixels32() : null;
        var rsPixels = rs != null ? rs.GetPixels32() : null;

        var outPixels = new Color32[w * h];
        for (int i = 0; i < outPixels.Length; i++)
        {
            byte rMetal = mPixels != null ? mPixels[i].r : (byte)0;     // default: non-metal
            byte aChan = rsPixels != null ? rsPixels[i].r : (byte)0;   // default: smoothness 0 (fully rough)

            // If input was ROUGHNESS, convert to SMOOTHNESS = 1 - roughness
            if (invertAlpha) aChan = (byte)(255 - aChan);

            outPixels[i] = new Color32(rMetal, 0, 0, aChan);            // R = metallic, A = smoothness
        }
        outTex.SetPixels32(outPixels);
        outTex.Apply(true, false);

        string savePath = $"{folder}/{root}_MetalR_SmoothA.png";
        File.WriteAllBytes(savePath, outTex.EncodeToPNG());
        AssetDatabase.ImportAsset(savePath, ImportAssetOptions.ForceUpdate);

        var ti = (TextureImporter)AssetImporter.GetAtPath(savePath);
        ti.sRGBTexture = false;                       // data map
        ti.alphaSource = TextureImporterAlphaSource.FromInput;
        ti.mipmapEnabled = true;
        ti.textureCompression = TextureImporterCompression.CompressedHQ;
        ti.isReadable = false;
        ti.SaveAndReimport();

        return AssetDatabase.LoadAssetAtPath<Texture2D>(savePath);
    }

    private static Texture2D EnsureReadable(Texture2D src, int targetW, int targetH)
    {
        if (src == null) return null;
        string path = AssetDatabase.GetAssetPath(src);
        var ti = (TextureImporter)AssetImporter.GetAtPath(path);
        bool changed = false;

        if (!ti.isReadable) { ti.isReadable = true; changed = true; }
        if (ti.sRGBTexture) { /* linear for data maps */ ti.sRGBTexture = false; changed = true; }
        if (changed) ti.SaveAndReimport();

        // Upscale/downscale if needed via RT
        if (src.width == targetW && src.height == targetH) return src;

        var rt = new RenderTexture(targetW, targetH, 0, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Linear);
        Graphics.Blit(src, rt);
        var tmp = new Texture2D(targetW, targetH, TextureFormat.RGBA32, false, true);
        var prev = RenderTexture.active;
        RenderTexture.active = rt;
        tmp.ReadPixels(new Rect(0, 0, targetW, targetH), 0, 0);
        tmp.Apply();
        RenderTexture.active = prev;
        rt.Release();
        return tmp;
    }

    // ---------- Texture discovery ----------
    private enum TexKind { Metallic, Roughness, Smoothness, Height, AO, Emissive }

    private static Texture2D FindTex(string folder, string root, TexKind kind)
    {
        var guids = AssetDatabase.FindAssets("t:Texture2D", new[] { folder });
        foreach (var g in guids)
        {
            var path = AssetDatabase.GUIDToAssetPath(g);
            var name = Path.GetFileNameWithoutExtension(path);
            if (!NameMatchesRoot(name, root)) continue;

            if (kind == TexKind.Metallic   && ContainsAny(name, "metal", "metallic", "metalness")) return Load(path);
            if (kind == TexKind.Roughness  && ContainsAny(name, "rough", "roughness"))             return Load(path);
            if (kind == TexKind.Smoothness && ContainsAny(name, "smooth", "smoothness", "gloss"))  return Load(path);
            if (kind == TexKind.Height     && ContainsAny(name, "height", "disp", "displace", "parallax")) return Load(path);
            if (kind == TexKind.AO         && ContainsAny(name, "_ao", "ao", "occlusion", "ambocc", "ambientocclusion")) return Load(path);
            if (kind == TexKind.Emissive   && ContainsAny(name, "emiss", "emission", "emissive"))  return Load(path);
        }
        return null;

        static Texture2D Load(string p) => AssetDatabase.LoadAssetAtPath<Texture2D>(p);
    }

    private static bool ContainsAny(string s, params string[] keys)
    {
        s = s.ToLowerInvariant();
        return keys.Any(k => s.Contains(k.ToLowerInvariant()));
    }

    private static bool NameLooksSmoothness(string path)
    {
        var n = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
        return n.Contains("smooth") || n.Contains("gloss");
    }

    private static bool NameMatchesRoot(string name, string root)
    {
        // Accept exact root + suffix, or the typical _mtl_* pattern after root
        if (name.Equals(root, System.StringComparison.OrdinalIgnoreCase)) return true;
        if (name.StartsWith(root + "_", System.StringComparison.OrdinalIgnoreCase)) return true;
        if (name.StartsWith(root + "-", System.StringComparison.OrdinalIgnoreCase)) return true;
        return false;
    }

    // Try to peel common base-color suffixes to get a stable root
    private static string GuessRootName(string baseName)
    {
        string[] suffixes =
        {
            "_BaseColor", "_Base_Color", "_Albedo", "_Color", "_Diffuse", "_Base", "_col",
            "_mtl_BaseColor", "_mtl_Base", "_mtl_Albedo",
            "-BaseColor", "-Albedo", "-Color"
        };

        foreach (var s in suffixes)
        {
            if (baseName.EndsWith(s, System.StringComparison.OrdinalIgnoreCase))
                return baseName.Substring(0, baseName.Length - s.Length);
        }
        return baseName;
    }

    // ---------- Material property helpers ----------
    private static bool TryGetBaseMap(Material m, out Texture2D tex)
    {
        tex = null;
        string[] props = { "_BaseMap", "_MainTex", "_BaseColorMap", "_BaseTex" };
        foreach (var p in props)
        {
            if (m.HasProperty(p))
            {
                tex = m.GetTexture(p) as Texture2D;
                if (tex != null) return true;
            }
        }
        return false;
    }

    private static bool TryGetMetalMapProp(Material m, out string prop)
    {
        string[] props = { "_MetallicGlossMap", "_MetallicMap", "_MaskMap", "_MetallicTex" };
        foreach (var p in props) if (m.HasProperty(p)) { prop = p; return true; }
        prop = null; return false;
    }

    private static bool TryGetParallaxProp(Material m, out string prop)
    {
        string[] props = { "_ParallaxMap", "_HeightMap", "_HeightTex" };
        foreach (var p in props) if (m.HasProperty(p)) { prop = p; return true; }
        prop = null; return false;
    }

    private static bool TryGetAOProp(Material m, out string prop)
    {
        string[] props = { "_OcclusionMap", "_AOMap", "_OcclusionTex" };
        foreach (var p in props) if (m.HasProperty(p)) { prop = p; return true; }
        prop = null; return false;
    }

    private static bool TryGetEmissionProps(Material m, out string mapProp, out string colorProp)
    {
        string[] maps = { "_EmissionMap", "_EmissiveMap", "_EmissionTex", "_EmissiveTex" };
        string[] cols = { "_EmissionColor", "_EmissiveColor", "_Emission", "_Emissive" };

        mapProp = maps.FirstOrDefault(m.HasProperty);
        colorProp = cols.FirstOrDefault(m.HasProperty);
        return mapProp != null && colorProp != null;
    }

    // ---------- Import settings ----------
    private static void ForceSRGB(Texture2D tex, bool sRGB)
    {
        if (tex == null) return;
        var path = AssetDatabase.GetAssetPath(tex);
        var ti = (TextureImporter)AssetImporter.GetAtPath(path);
        if (ti == null) return;

        bool changed = false;
        if (ti.sRGBTexture != sRGB) { ti.sRGBTexture = sRGB; changed = true; }

        // If it's clearly a normal map, keep it marked as NormalMap and linear
        var lower = Path.GetFileNameWithoutExtension(path).ToLowerInvariant();
        if (lower.Contains("normal") || lower.Contains("_n"))
        {
            if (ti.textureType != TextureImporterType.NormalMap)
            {
                ti.textureType = TextureImporterType.NormalMap;
                changed = true;
            }
            ti.sRGBTexture = false;
        }

        if (changed) ti.SaveAndReimport();
    }
}
