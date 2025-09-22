
using UnityEngine;

[System.Serializable]
public class GameAction_SetMaterialProperty : GameAction
{
    public enum  Type 
    {
        Float, Color, Vector, Texture
    };

    [SerializeField] private Type       type;
    [SerializeField] private string     propName;
    [SerializeField] private Color      color;
    [SerializeField] private Vector4    vector;
    [SerializeField] private float      fValue;
    [SerializeField] private Texture    texture;

    public override void Execute(GameObject go)
    {
        Material material = null;
        Renderer renderer = go.GetComponent<Renderer>();
        if (renderer != null)
        {
            material = renderer.material;
        }
        
        if (material == null)
        {
            return;
        }

        switch (type)
        {
            case Type.Float:
                material.SetFloat(propName, fValue);
                break;
            case Type.Color:
                material.SetColor(propName, color);
                break;
            case Type.Vector:
                material.SetVector(propName, vector);
                break;
            case Type.Texture:
                material.SetTexture(propName, texture);
                break;
            default:
                break;
        }
    }
}
