// Assets/Editor/Condition_IsEquippedDrawer.cs
using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(WSKit.Condition_IsEquipped))]
    public class Condition_IsEquippedDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var negateProp = property.FindPropertyRelative("negate");
            var slotProp = property.FindPropertyRelative("slot"); // Hypertag
            var itemProp = property.FindPropertyRelative("item"); // Item

            const float notW = 48f;  // same as your working drawer
            const float pad = 4f;

            // Left: NOT toggle
            var notRect = new Rect(position.x, position.y, notW, position.height);
            var restRect = new Rect(position.x + notW, position.y, position.width - notW, position.height);

            negateProp.boolValue = EditorGUI.ToggleLeft(notRect, "Not", negateProp.boolValue);

            // Label (aligned with other inspectors)
            float labelWidth = Mathf.Max(50f, EditorGUIUtility.labelWidth - notW);
            var labelRect = new Rect(restRect.x, restRect.y, labelWidth, restRect.height);
            var fieldsRect = new Rect(restRect.x + labelWidth, restRect.y, restRect.width - labelWidth, restRect.height);

            // Keep the label style consistent with others (use provided label)
            // If you prefer a hardcoded text, replace `label` with: new GUIContent("Is Equipped")
            EditorGUI.LabelField(labelRect, label);

            // Split fieldsRect into [slot] [item]
            float slotW = Mathf.Max(90f, fieldsRect.width * 0.40f);
            float itemW = Mathf.Max(90f, fieldsRect.width - slotW - pad);

            var slotRect = new Rect(fieldsRect.x, fieldsRect.y, slotW, fieldsRect.height);
            var itemRect = new Rect(slotRect.xMax + pad, fieldsRect.y, itemW, fieldsRect.height);

            EditorGUI.PropertyField(slotRect, slotProp, GUIContent.none);
            EditorGUI.PropertyField(itemRect, itemProp, GUIContent.none);
        }
    }
}
