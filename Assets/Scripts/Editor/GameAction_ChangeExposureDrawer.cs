// Assets/Editor/GameAction_ToggleInputDrawer.cs
using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_ChangeExposure))]
    public class GameAction_ChangeExposureDrawer : BaseGameActionDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
            => EditorGUIUtility.singleLineHeight;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var exposureProp = property.FindPropertyRelative("exposure");
            var durationProp = property.FindPropertyRelative("duration");

            // Split into: content area + right-aligned wait column
            SplitForWait(position, out var content, out var waitRect);

            float h = EditorGUIUtility.singleLineHeight;
            float labelW = EditorGUIUtility.labelWidth;

            var labelRect = new Rect(content.x, content.y, labelW, h);
            var restRect = new Rect(labelRect.xMax, content.y, content.width - labelW, h);
            var exposureRect = new Rect(restRect.x, restRect.y, restRect.width * 0.4f, restRect.height);
            var inRect = new Rect(exposureRect.xMax + restRect.width * 0.05f, restRect.y, restRect.width * 0.05f, restRect.height);
            var durationRect = new Rect(inRect.xMax, restRect.y, restRect.width * 0.4f, restRect.height);
            var secondRect = new Rect(durationRect.xMax, restRect.y, restRect.width * 0.1f, restRect.height);

            EditorGUI.BeginProperty(position, label, property);

            // Standard inspector label (matches style with others)
            EditorGUI.LabelField(labelRect, "Change Exposure");

            // Enum fills remaining content width
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(exposureRect, exposureProp, GUIContent.none);
            EditorGUI.LabelField(inRect, new GUIContent("in"));
            EditorGUI.PropertyField(durationRect, durationProp, GUIContent.none);
            EditorGUI.LabelField(secondRect, new GUIContent("sec"));
            EditorGUI.indentLevel = oldIndent;

            // Wait checkbox stays aligned on the right
            DrawWait(waitRect, property);

            EditorGUI.EndProperty();
        }
    }
}
