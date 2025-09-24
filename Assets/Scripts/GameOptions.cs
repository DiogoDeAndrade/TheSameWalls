using TMPro;
using UnityEngine;

[CreateAssetMenu(fileName = "GameOptions", menuName = "One Room/Game Options")]
public class GameOptions : ScriptableObject
{
    public bool     dyslexicMode = false;
    public float    textScale = 1.0f;

    [SerializeField] TMP_FontAsset normalFont;
    [SerializeField] Material      normalMaterial;
    [SerializeField] TMP_FontAsset dyslexicFont;
    [SerializeField] Material      dyslexicMaterial;

    public TMP_FontAsset GetTextFont()
    {
        return (dyslexicMode) ? dyslexicFont : normalFont;
    }

    public Material GetTextMaterial()
    {
        return (dyslexicMode) ? dyslexicMaterial : normalMaterial;
    }
}
