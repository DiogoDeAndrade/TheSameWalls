// Assets/Editor/ConditionHasItemDrawer.cs
using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(WSKit.Condition_HasItem))]
    public class ConditionHasItemDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var negateProp = property.FindPropertyRelative("negate");
            var itemProp = property.FindPropertyRelative("item");

            // NOT checkbox
            const float notW = 48f;
            var notRect = new Rect(position.x, position.y, notW, position.height);
            var restRect = new Rect(position.x + notW, position.y, position.width - notW, position.height);

            negateProp.boolValue = EditorGUI.ToggleLeft(notRect, "Not", negateProp.boolValue);

            // Label + field (snug)
            float labelWidth = Mathf.Max(50f, EditorGUIUtility.labelWidth - notW);
            var labelRect = new Rect(restRect.x, restRect.y, labelWidth, restRect.height);
            var fieldRect = new Rect(restRect.x + labelWidth, restRect.y, restRect.width - labelWidth, restRect.height);

            EditorGUI.LabelField(labelRect, label);                // e.g., "Has Item"
            EditorGUI.PropertyField(fieldRect, itemProp, GUIContent.none);
        }
    }
}
