// Assets/Editor/GameActionSetMaterialPropertyDrawer.cs
using UnityEditor;
using UnityEngine;
using UC.Interaction.Editor;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_SetAnimatorProperty))]
    public class GameAction_SetAnimatorPropertyDrawer : BaseGameActionDrawer
    {
        const float Pad = 2f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
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

            var nameProp = property.FindPropertyRelative("propName");
            var fValue = property.FindPropertyRelative("fValue");
            var iValue = property.FindPropertyRelative("iValue");
            var bValue = property.FindPropertyRelative("bValue");
            var animatorProp = property.FindPropertyRelative("animator");

            var r = new Rect(position.x, position.y + Pad, position.width, EditorGUIUtility.singleLineHeight);

            // Line 1: Type (left) + Wait (right)
            SplitForWait(r, out var left, out var wait);
            EditorGUI.PropertyField(left, animatorProp, new GUIContent("Animator"));
            DrawWait(wait, property);
            r = NextLine(r);

            // Line 2: Property Name (full width)
            EditorGUI.PropertyField(r, nameProp, GUIContent.none);
            r = NextLine(r);

            // Line 3: Value (based on type)
            // Access the **object** behind the property
            var instance = property.managedReferenceValue as GameAction_SetAnimatorProperty;

            // Fallback for non-managed fields (rare here) using reflection:
            if (instance == null && fieldInfo != null)
            {
                instance = fieldInfo.GetValue(property.serializedObject.targetObject) as GameAction_SetAnimatorProperty;
            }

            var type = AnimatorControllerParameterType.Trigger;
            if (instance != null)
            {
                var p = instance.propType;
                if (p != null)
                {
                    type = p.Value;

                    switch (type)
                    {
                        case AnimatorControllerParameterType.Float:
                            EditorGUI.PropertyField(r, fValue, new GUIContent("Value"));
                            break;
                        case AnimatorControllerParameterType.Int:
                            EditorGUI.PropertyField(r, iValue, new GUIContent("Value"));
                            break;
                        case AnimatorControllerParameterType.Bool:
                            EditorGUI.PropertyField(r, bValue, new GUIContent("Value"));
                            break;
                        case AnimatorControllerParameterType.Trigger:
                            break;
                        default:
                            break;
                    }
                }
            }

            EditorGUI.EndProperty();
        }
    }
}
