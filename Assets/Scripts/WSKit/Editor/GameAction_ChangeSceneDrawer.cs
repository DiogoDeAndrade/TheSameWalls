using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_ChangeScene))]
    public class GameAction_ChangeSceneDrawer : BaseGameActionDrawer
    {
        static readonly float Line = EditorGUIUtility.singleLineHeight;
        const float VSpace = 2f;
        const float WaitW = 20f;
        const float Pad = 4f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // header + one row (fade time & color)
            return Line + VSpace + Line;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var sceneProp = property.FindPropertyRelative("scene");
            var fadeTimeProp = property.FindPropertyRelative("fadeTime");
            var fadeColorProp = property.FindPropertyRelative("fadeColor");
            var waitProp = property.FindPropertyRelative("wait"); // may be null in some actions

            var header = new Rect(position.x, position.y, position.width, Line);
            var row = new Rect(position.x, position.y + Line + VSpace, position.width, Line);

            // Header rects: [Scene selector] [ "Change Scene" label ] .................. [Wait]
            Rect waitRect = new Rect(header.xMax - WaitW, header.y, WaitW, Line);
            float rightEdgeForTitle = waitRect.x - Pad;

            // Give the scene selector a generous slice, then title takes the rest
            float sceneW = Mathf.Max(120f, header.width * 0.55f - WaitW);
            Rect sceneRect = new Rect(header.x, header.y, Mathf.Min(sceneW, rightEdgeForTitle - header.x), Line);
            Rect titleRect = new Rect(sceneRect.xMax + Pad, header.y, Mathf.Max(0f, rightEdgeForTitle - (sceneRect.xMax + Pad)), Line);

            EditorGUI.BeginProperty(position, label, property);

            // Scene selector (no label)
            if (sceneProp != null)
                EditorGUI.PropertyField(sceneRect, sceneProp, GUIContent.none);

            // Title
            EditorGUI.LabelField(titleRect, "Change Scene");

            // Right-aligned wait checkbox (if present)
            if (waitProp != null)
                EditorGUI.PropertyField(waitRect, waitProp, GUIContent.none);

            // Second row: Fade Time | Fade Color
            float half = (row.width - Pad) * 0.5f;
            var rTime = new Rect(row.x, row.y, half, Line);
            var rColor = new Rect(row.x + half + Pad, row.y, row.width - half - Pad, Line);

            float oldLW = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 70f;
            if (fadeTimeProp != null) EditorGUI.PropertyField(rTime, fadeTimeProp, new GUIContent("Fade Time"));
            if (fadeColorProp != null) EditorGUI.PropertyField(rColor, fadeColorProp, new GUIContent("Fade Color"));
            EditorGUIUtility.labelWidth = oldLW;

            EditorGUI.EndProperty();
        }
    }
}
