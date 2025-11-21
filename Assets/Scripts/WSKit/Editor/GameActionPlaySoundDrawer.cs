
using UnityEditor;
using UnityEngine;
using UC.Interaction.Editor;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_PlaySound))]
    public class GameAction_PlaySoundDrawer : BaseGameActionDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Single line only
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var tagProp = property.FindPropertyRelative("sound");

            // Split the line into the left content and right 'wait' area
            SplitForWait(position, out var contentRect, out var waitRect);

            // Split rect into label + field
            float labelWidth = EditorGUIUtility.labelWidth; // default label width
            var labelRect = new Rect(contentRect.x, contentRect.y, labelWidth, contentRect.height);
            var fieldRect = new Rect(contentRect.x + labelWidth, contentRect.y,
                                     contentRect.width - labelWidth, contentRect.height);

            EditorGUI.LabelField(labelRect, label); // "sound"
            EditorGUI.PropertyField(fieldRect, tagProp, GUIContent.none);

            // Finally, draw the wait checkbox on the right
            DrawWait(waitRect, property);
        }
    }
}