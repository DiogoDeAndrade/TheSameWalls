
using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_AddToken))]
    public class GameAction_AddTokenDrawer : BaseGameActionDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Single line only
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var tokenProp = property.FindPropertyRelative("token");
            var quantityProp = property.FindPropertyRelative("quantity");

            // Split the line into the left content and right 'wait' area
            SplitForWait(position, out var contentRect, out var waitRect);

            // Split rect into label + field
            float labelWidth = EditorGUIUtility.labelWidth; // default label width
            var labelRect = new Rect(contentRect.x, contentRect.y, labelWidth, contentRect.height);
            var fieldRect = new Rect(contentRect.x + labelWidth, contentRect.y,
                                     contentRect.width - labelWidth, contentRect.height);

            var quantityRect = new Rect(fieldRect.x, fieldRect.y, 64, fieldRect.height); ;
            var timesRect = new Rect(quantityRect.x + quantityRect.width + 5, quantityRect.y, 16, quantityRect.height);
            var tokenRect = new Rect(timesRect.x + timesRect.width, timesRect.y, fieldRect.width - quantityRect.width - 16, timesRect.height);

            EditorGUI.LabelField(labelRect, label); // "sound"
            EditorGUI.PropertyField(quantityRect, quantityProp, GUIContent.none);
            EditorGUI.LabelField(timesRect, "x"); // "sound"
            EditorGUI.PropertyField(tokenRect, tokenProp, GUIContent.none);

            // Finally, draw the wait checkbox on the right
            DrawWait(waitRect, property);
        }
    }
}