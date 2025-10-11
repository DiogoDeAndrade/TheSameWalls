// Assets/Editor/GameAction_ChangeSceneDrawer.cs
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
            // 2 lines: header + fade line
            return Line + VSpace + Line;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var sceneProp = property.FindPropertyRelative("scene");
            var fadeTimeProp = property.FindPropertyRelative("fadeTime");
            var fadeColorProp = property.FindPropertyRelative("fadeColor");
            var waitProp = property.FindPropertyRelative("wait");

            var header = new Rect(position.x, position.y, position.width, Line);
            var row = new Rect(position.x, position.y + Line + VSpace, position.width, Line);

            // Split first line into: [Label "Change Scene"] [Scene selector] [Wait]
            Rect waitRect = new Rect(header.xMax - WaitW, header.y, WaitW, Line);
            float rightEdge = waitRect.x - Pad;

            // Label area (fixed width)
            float labelW = EditorGUIUtility.labelWidth;
            Rect labelRect = new Rect(header.x, header.y, labelW, Line);

            // Scene selector takes the rest
            Rect sceneRect = new Rect(labelRect.xMax, header.y, rightEdge - labelRect.xMax, Line);

            EditorGUI.BeginProperty(position, label, property);

            // Label
            EditorGUI.LabelField(labelRect, "Change Scene");

            // Force the Scene selector to use the full rect (NaughtyAttributes-safe)
            float oldLW = EditorGUIUtility.labelWidth;
            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUIUtility.labelWidth = 1f; // effectively no label space

            EditorGUI.PropertyField(sceneRect, sceneProp, GUIContent.none);

            EditorGUIUtility.labelWidth = oldLW;
            EditorGUI.indentLevel = oldIndent;

            // Wait checkbox
            if (waitProp != null)
                EditorGUI.PropertyField(waitRect, waitProp, GUIContent.none);

            // Second line — Fade Time / Fade Color
            float half = (row.width - Pad) * 0.5f;
            var rTime = new Rect(row.x, row.y, half, Line);
            var rColor = new Rect(row.x + half + Pad, row.y, row.width - half - Pad, Line);

            oldLW = EditorGUIUtility.labelWidth;
            EditorGUIUtility.labelWidth = 70f;

            if (fadeTimeProp != null)
                EditorGUI.PropertyField(rTime, fadeTimeProp, new GUIContent("Fade Time"));

            if (fadeColorProp != null)
                EditorGUI.PropertyField(rColor, fadeColorProp, new GUIContent("Fade Color"));

            EditorGUIUtility.labelWidth = oldLW;

            EditorGUI.EndProperty();
        }
    }
}
