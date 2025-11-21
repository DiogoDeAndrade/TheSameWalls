
using System.Collections;
using UnityEngine;

namespace WSKit
{

    [System.Serializable]
    [GameActionName("WSL/Set Material Property")]
    public class GameAction_SetMaterialProperty : GameAction
    {
        
        public enum Type
        {
            Float, Color, Vector, Texture
        };

        [SerializeField] private TargetMaterial targetMaterial;
        [SerializeField] private Type           type;
        [SerializeField] private string         propName;
        [SerializeField] private Color          color;
        [SerializeField] private Vector4        vector;
        [SerializeField] private float          fValue;
        [SerializeField] private Texture        texture;

        public override IEnumerator Execute(GameObject go)
        {
            Renderer renderer = null;
            Material material = null;
            switch (targetMaterial.target)
            {
                case TargetMaterial.Target.ThisRenderer:
                    renderer = go.GetComponent<Renderer>();
                    if (renderer != null)
                    {
                        material = renderer.materials[targetMaterial.subMaterial];
                    }
                    break;
                case TargetMaterial.Target.Material:
                    material = targetMaterial.material;
                    break;
                case TargetMaterial.Target.Renderer:
                    renderer = targetMaterial.renderer;
                    if (renderer != null)
                    {
                        material = renderer.materials[targetMaterial.subMaterial];
                    }
                    break;
                default:
                    break;
            }

            if (material == null)
            {
                yield return null;
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

            yield return null;
        }
    }
}