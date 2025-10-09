using System;
using UnityEngine;

[Serializable]
public struct TargetMaterial
{
    public enum Target
    {
        ThisRenderer,
        Material,
        Renderer,
    };

    public Target     target;
    public Material   material;
    public Renderer   renderer;
    public int        subMaterial;
}
