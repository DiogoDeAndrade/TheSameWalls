// Assets/Editor/GameActionSetMaterialPropertyDrawer.cs
using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_SetMaterialProperty))]
    public class GameAction_SetMaterialPropertyDrawer : BaseGameActionDrawer
    {
        const float Pad = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var typeProp = property.FindPropertyRelative("type");

            // Lines: Type/Wait + Property Name + Value (one line)
            int rows = 2; // type + prop name
            rows += 1;    // value line (float/color/vector/texture)

            return rows * EditorGUIUtility.singleLineHeight
                 + (rows - 1) * EditorGUIUtility.standardVerticalSpacing
                 + Pad * 2f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);

            var typeProp = property.FindPropertyRelative("type");
            var nameProp = property.FindPropertyRelative("propName");
            var colorProp = property.FindPropertyRelative("color");
            var vectorProp = property.FindPropertyRelative("vector");
            var floatProp = property.FindPropertyRelative("fValue");
            var texProp = property.FindPropertyRelative("texture");

            var r = new Rect(position.x, position.y + Pad, position.width, EditorGUIUtility.singleLineHeight);

            // Line 1: Type (left) + Wait (right)
            SplitForWait(r, out var left, out var wait);
            EditorGUI.PropertyField(left, typeProp, new GUIContent("Change Material Property"));
            DrawWait(wait, property);
            r = NextLine(r);

            // Line 2: Property Name (full width)
            EditorGUI.PropertyField(r, nameProp, new GUIContent("Property Name"));
            r = NextLine(r);

            // Line 3: Value (based on type)
            switch ((WSKit.GameAction_SetMaterialProperty.Type)typeProp.enumValueIndex)
            {
                case WSKit.GameAction_SetMaterialProperty.Type.Float:
                    EditorGUI.PropertyField(r, floatProp, new GUIContent("Value"));
                    break;

                case WSKit.GameAction_SetMaterialProperty.Type.Color:
                    {
                        var current = colorProp.colorValue;
                        var newCol = EditorGUI.ColorField(r, new GUIContent("Color (HDR)"),
                            current, showEyedropper: true, showAlpha: true, hdr: true);
                        if (newCol != current) colorProp.colorValue = newCol;
                        break;
                    }

                case WSKit.GameAction_SetMaterialProperty.Type.Vector:
                    {
                        var v = vectorProp.vector4Value;
                        v = EditorGUI.Vector4Field(r, "Vector", v);
                        if (v != vectorProp.vector4Value) vectorProp.vector4Value = v;
                        break;
                    }

                case WSKit.GameAction_SetMaterialProperty.Type.Texture:
                    EditorGUI.PropertyField(r, texProp, new GUIContent("Texture"));
                    break;
            }

            EditorGUI.EndProperty();
        }
    }
}
