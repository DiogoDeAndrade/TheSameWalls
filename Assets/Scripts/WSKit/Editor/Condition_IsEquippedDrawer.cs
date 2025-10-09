// Assets/Editor/Condition_IsEquippedDrawer.cs
using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(Condition_IsEquipped))]
    public class Condition_IsEquippedDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var notProp = property.FindPropertyRelative("not");   // from base Condition
            var slotProp = property.FindPropertyRelative("slot");  // Hypertag
            var itemProp = property.FindPropertyRelative("item");  // Item

            float h = EditorGUIUtility.singleLineHeight;
            float pad = 4f;

            // Keep alignment with other inspectors
            float labelWidth = EditorGUIUtility.labelWidth;

            // Small inline NOT toggle area
            float notW = 40f; // room for a tiny "NOT" text + checkbox

            // Line layout
            var labelRect = new Rect(position.x, position.y, Mathf.Max(60f, labelWidth - notW - pad), h);
            var notRect = new Rect(labelRect.xMax + pad, position.y, notW, h);

            // Remaining width for fields
            float remainingX = notRect.xMax + pad;
            float remainingW = position.xMax - remainingX;

            // Split remaining: slot (40%) | item (60%)
            float slotW = Mathf.Max(90f, remainingW * 0.4f);
            float itemW = Mathf.Max(90f, remainingW - slotW - pad);

            var slotRect = new Rect(remainingX, position.y, slotW, h);
            var itemRect = new Rect(slotRect.xMax + pad, position.y, itemW, h);

            EditorGUI.BeginProperty(position, label, property);

            // Label
            EditorGUI.LabelField(labelRect, "Is Equipped");

            // NOT (checkbox + tiny label)
            if (notProp != null)
            {
                // draw checkbox first, then the word "NOT"
                float box = h; // square box height
                var boxRect = new Rect(notRect.x, position.y + (h - box) * 0.5f, box, box);
                notProp.boolValue = EditorGUI.Toggle(boxRect, notProp.boolValue);

                var notLbl = new Rect(boxRect.xMax + 2f, position.y, notRect.xMax - (boxRect.xMax + 2f), h);
                EditorGUI.LabelField(notLbl, "NOT", EditorStyles.miniLabel);
            }

            // Fields (no labels)
            EditorGUI.PropertyField(slotRect, slotProp, GUIContent.none);
            EditorGUI.PropertyField(itemRect, itemProp, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}
