using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_SetMaterialProperty))]
    public class GameAction_SetMaterialPropertyDrawer : BaseGameActionDrawer
    {
        const float Pad = 2f;
        const float ExtraLineGap = 3f; // small extra spacing between line 1 and 2

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // 2 lines + tiny extra gap
            int rows = 2;
            return rows * EditorGUIUtility.singleLineHeight
                 + (rows - 1) * EditorGUIUtility.standardVerticalSpacing
                 + Pad * 2f
                 + ExtraLineGap;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var targetMatProp = property.FindPropertyRelative("targetMaterial");
            var typeProp = property.FindPropertyRelative("type");
            var nameProp = property.FindPropertyRelative("propName");
            var colorProp = property.FindPropertyRelative("color");
            var vectorProp = property.FindPropertyRelative("vector");
            var floatProp = property.FindPropertyRelative("fValue");
            var texProp = property.FindPropertyRelative("texture");

            var r = new Rect(position.x, position.y + Pad, position.width, EditorGUIUtility.singleLineHeight);

            // Line 1: "Set Material Property" + targetMaterial + wait
            SplitForWait(r, out var content, out var waitRect);

            float labelWidth = EditorGUIUtility.labelWidth;
            var labelRect = new Rect(content.x, content.y, labelWidth, content.height);
            var fieldRect = new Rect(labelRect.xMax, content.y, content.width - labelWidth, content.height);

            EditorGUI.LabelField(labelRect, "Set Material Property");
            EditorGUI.PropertyField(fieldRect, targetMatProp, GUIContent.none);
            DrawWait(waitRect, property);

            // Line 2: add a tiny extra gap
            r = NextLine(r);
            r.y += ExtraLineGap;

            // We want the right edge to align with the end of line 1 content (i.e., reserve WaitColumnWidth)
            float usableWidth = position.width - WaitColumnWidth;
            var rAligned = new Rect(position.x, r.y, usableWidth, r.height);

            // "Property [Type] [Name] = [Value]"
            const float propLabelW = 70f;
            const float spacing = 4f;

            var propLabelRect = new Rect(rAligned.x, rAligned.y, propLabelW, rAligned.height);
            EditorGUI.LabelField(propLabelRect, "Property");

            float typeW = 100f;
            var typeRect = new Rect(propLabelRect.xMax + spacing, rAligned.y, typeW, rAligned.height);
            EditorGUI.PropertyField(typeRect, typeProp, GUIContent.none);

            float nameMinW = 120f;
            var nameRect = new Rect(typeRect.xMax + spacing, rAligned.y, Mathf.Max(nameMinW, 160f), rAligned.height);
            EditorGUI.PropertyField(nameRect, nameProp, GUIContent.none);

            var equalsRect = new Rect(nameRect.xMax + spacing, rAligned.y, 12f, rAligned.height);
            EditorGUI.LabelField(equalsRect, "=");

            var valueRect = new Rect(equalsRect.xMax + spacing, rAligned.y, rAligned.xMax - (equalsRect.xMax + spacing), rAligned.height);

            switch ((WSKit.GameAction_SetMaterialProperty.Type)typeProp.enumValueIndex)
            {
                case WSKit.GameAction_SetMaterialProperty.Type.Float:
                    EditorGUI.PropertyField(valueRect, floatProp, GUIContent.none);
                    break;

                case WSKit.GameAction_SetMaterialProperty.Type.Color:
                    {
                        var col = colorProp.colorValue;
                        var newCol = EditorGUI.ColorField(valueRect, GUIContent.none, col, true, true, true);
                        if (newCol != col) colorProp.colorValue = newCol;
                        break;
                    }

                case WSKit.GameAction_SetMaterialProperty.Type.Vector:
                    {
                        var v = vectorProp.vector4Value;
                        v = EditorGUI.Vector4Field(valueRect, GUIContent.none, v);
                        if (v != vectorProp.vector4Value) vectorProp.vector4Value = v;
                        break;
                    }

                case WSKit.GameAction_SetMaterialProperty.Type.Texture:
                    EditorGUI.PropertyField(valueRect, texProp, GUIContent.none);
                    break;
            }

            EditorGUI.EndProperty();
        }
    }
}
