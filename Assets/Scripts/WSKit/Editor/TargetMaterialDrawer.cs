using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(TargetMaterial))]
public class TargetMaterialDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => EditorGUIUtility.singleLineHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var targetProp = property.FindPropertyRelative("target");
        var materialProp = property.FindPropertyRelative("material");
        var rendererProp = property.FindPropertyRelative("renderer");
        var subMatProp = property.FindPropertyRelative("subMaterial");

        position.height = EditorGUIUtility.singleLineHeight;
        var contentRect = EditorGUI.PrefixLabel(position, label);

        const float pad = 4f;
        const float popupWidth = 110f;
        const float idxLabelW = 28f;
        const float idxPad = 2f;

        // Target popup
        var targetRect = new Rect(contentRect.x, contentRect.y, popupWidth, contentRect.height);
        EditorGUI.PropertyField(targetRect, targetProp, GUIContent.none);

        var remain = new Rect(targetRect.xMax + pad, contentRect.y,
                              contentRect.width - popupWidth - pad, contentRect.height);
        if (remain.width <= 0f) return;

        var t = (TargetMaterial.Target)targetProp.enumValueIndex;
        float subW = Mathf.Min(70f, remain.width - idxLabelW - idxPad);

        switch (t)
        {
            case TargetMaterial.Target.Material:
                // Only Material selector
                EditorGUI.PropertyField(remain, materialProp, GUIContent.none);
                break;

            case TargetMaterial.Target.ThisRenderer:
                {
                    // IDX + subMaterial
                    var idxRect = new Rect(remain.x, remain.y, idxLabelW, remain.height);
                    var subRect = new Rect(idxRect.xMax + idxPad, remain.y,
                                           remain.width - idxLabelW - idxPad, remain.height);
                    EditorGUI.LabelField(idxRect, "IDX");
                    EditorGUI.PropertyField(subRect, subMatProp, GUIContent.none);
                    break;
                }

            case TargetMaterial.Target.Renderer:
                {
                    // Always show the Renderer selector + IDX + subMaterial
                    var rendRect = new Rect(remain.x, remain.y,
                                            remain.width - subW - idxLabelW - idxPad - pad, remain.height);
                    var idxRect = new Rect(rendRect.xMax + pad, remain.y, idxLabelW, remain.height);
                    var subRect = new Rect(idxRect.xMax + idxPad, remain.y, subW, remain.height);

                    EditorGUI.PropertyField(rendRect, rendererProp, GUIContent.none);
                    EditorGUI.LabelField(idxRect, "IDX");
                    EditorGUI.PropertyField(subRect, subMatProp, GUIContent.none);
                    break;
                }
        }
    }
}
