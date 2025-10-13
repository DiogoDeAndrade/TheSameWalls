// Assets/Editor/GameAction_ToggleInputDrawer.cs
using UnityEditor;
using UnityEngine;
using WSKit;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_ToggleInput))]
    public class GameAction_ToggleInputDrawer : BaseGameActionDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var stateProp = property.FindPropertyRelative("state");

            // Split into: content area + right-aligned wait column
            SplitForWait(position, out var content, out var waitRect);

            float h = EditorGUIUtility.singleLineHeight;
            float labelW = EditorGUIUtility.labelWidth;

            var labelRect = new Rect(content.x, content.y, labelW, h);
            var fieldRect = new Rect(labelRect.xMax, content.y, content.width - labelW, h);

            EditorGUI.BeginProperty(position, label, property);

            // Standard inspector label (matches style with others)
            EditorGUI.LabelField(labelRect, "Toggle Input");

            // Enum fills remaining content width
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(fieldRect, stateProp, GUIContent.none);
            EditorGUI.indentLevel = oldIndent;

            // Wait checkbox stays aligned on the right
            DrawWait(waitRect, property);

            EditorGUI.EndProperty();
        }
    }
}
