// Assets/Editor/GameActionSetMaterialPropertyDrawer.cs
using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_SetMaterialProperty))]
    public class GameActionSetMaterialPropertyDrawer : PropertyDrawer
    {
        const float Pad = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var typeProp = property.FindPropertyRelative("type");
            var propName = property.FindPropertyRelative("propName");

            int rows = 2; // Type + Property Name

            switch ((GameAction_SetMaterialProperty.Type)typeProp.enumValueIndex)
            {
                case GameAction_SetMaterialProperty.Type.Float: rows += 1; break;
                case GameAction_SetMaterialProperty.Type.Color: rows += 1; break;
                case GameAction_SetMaterialProperty.Type.Vector: rows += 1; break;
                case GameAction_SetMaterialProperty.Type.Texture: rows += 1; break;
            }

            float h = rows * EditorGUIUtility.singleLineHeight +
                      (rows - 1) * EditorGUIUtility.standardVerticalSpacing + Pad * 2f;

            // If prop name is empty, we’ll draw a warning box (extra height)
            if (string.IsNullOrEmpty(propName.stringValue))
            {
                h += EditorGUIUtility.singleLineHeight * 0.9f + EditorGUIUtility.standardVerticalSpacing;
            }

            return h;
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

            // Padded rect
            var r = new Rect(position.x, position.y + Pad, position.width, EditorGUIUtility.singleLineHeight);

            // 1) Type
            EditorGUI.PropertyField(r, typeProp, new GUIContent("Type"));
            r = NextLine(r);

            // 2) Property Name
            EditorGUI.PropertyField(r, nameProp, new GUIContent("Property Name"));
            r = NextLine(r);

            // Optional warning if name empty
            if (string.IsNullOrEmpty(nameProp.stringValue))
            {
                var warnRect = new Rect(r.x, r.y, r.width, EditorGUIUtility.singleLineHeight * 0.9f);
                EditorGUI.HelpBox(warnRect, "Shader property name is empty.", MessageType.Warning);
                r = NextLine(r);
            }

            // 3) Value (based on type)
            switch ((GameAction_SetMaterialProperty.Type)typeProp.enumValueIndex)
            {
                case GameAction_SetMaterialProperty.Type.Float:
                    {
                        EditorGUI.PropertyField(r, floatProp, new GUIContent("Value"));
                        break;
                    }
                case GameAction_SetMaterialProperty.Type.Color:
                    {
                        // HDR color picker
                        EditorGUI.BeginChangeCheck();
                        var current = colorProp.colorValue;
                        var newCol = EditorGUI.ColorField(r, new GUIContent("Color (HDR)"),
                            current, showEyedropper: true, showAlpha: true, hdr: true);
                        if (EditorGUI.EndChangeCheck())
                        {
                            colorProp.colorValue = newCol;
                        }
                        break;
                    }
                case GameAction_SetMaterialProperty.Type.Vector:
                    {
                        EditorGUI.BeginChangeCheck();
                        var v = vectorProp.vector4Value;
                        v = EditorGUI.Vector4Field(r, "Vector", v);
                        if (EditorGUI.EndChangeCheck())
                        {
                            vectorProp.vector4Value = v;
                        }
                        break;
                    }
                case GameAction_SetMaterialProperty.Type.Texture:
                    {
                        EditorGUI.ObjectField(
                            r,
                            texProp,
                            new GUIContent("Texture")
                        );
                        break;
                    }
            }

            EditorGUI.EndProperty();
        }

        private static Rect NextLine(Rect r)
        {
            r.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
            return r;
        }
    }
}