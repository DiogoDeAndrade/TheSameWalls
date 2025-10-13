// Assets/Editor/GameAction_ChangeSoundDrawer.cs
using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_ChangeSound))]
    public class GameAction_ChangeSoundDrawer : BaseGameActionDrawer
    {
        const float Pad = 4f;
        const float VGap2 = 3f;
        const float PropW = 90f;

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var propProp = property.FindPropertyRelative("property");
            int pIndex = propProp != null ? propProp.enumValueIndex : 0;
            bool singleLine = IsDisable(pIndex);
            int rows = singleLine ? 1 : 2;
            float h = rows * EditorGUIUtility.singleLineHeight
                    + (rows > 1 ? (EditorGUIUtility.standardVerticalSpacing + VGap2) : 0f);
            return h;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var tagProp = property.FindPropertyRelative("soundTag");
            var propProp = property.FindPropertyRelative("property");
            var targetProp = property.FindPropertyRelative("targetValue");
            var durationProp = property.FindPropertyRelative("duration");
            var stopProp = property.FindPropertyRelative("stopOnEnd");

            // 
            // Line 1: "Change Sound" [Tag] [Property] ... [Wait]
            // 
            var line1 = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
            SplitForWait(line1, out var c1, out var waitRect);

            float compactLabelW = Mathf.Min(110f, EditorGUIUtility.labelWidth);
            var labelRect = new Rect(c1.x, c1.y, compactLabelW, c1.height);
            var fieldsRect = new Rect(labelRect.xMax, c1.y, c1.width - compactLabelW, c1.height);

            var tagRect = new Rect(fieldsRect.x, fieldsRect.y, Mathf.Max(100f, fieldsRect.width - PropW - Pad), fieldsRect.height);
            var propRect = new Rect(tagRect.xMax + Pad, fieldsRect.y, PropW, fieldsRect.height);

            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.LabelField(labelRect, "Change Sound");

            int oldIndent = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
            EditorGUI.PropertyField(tagRect, tagProp, GUIContent.none);
            EditorGUI.PropertyField(propRect, propProp, GUIContent.none);
            DrawWait(waitRect, property);

            int pIndex = propProp.enumValueIndex;
            bool singleLine = IsDisable(pIndex);

            // 
            // Line 2: only if not Disable
            //
            if (!singleLine)
            {
                var line2StartX = tagRect.x;
                float rightEdge = position.x + position.width - WaitColumnWidth;
                var line2 = new Rect(line2StartX,
                                     line1.yMax + EditorGUIUtility.standardVerticalSpacing + VGap2,
                                     rightEdge - line2StartX,
                                     EditorGUIUtility.singleLineHeight);

                string valueLabel = (pIndex == 0) ? "Volume" :
                                    (pIndex == 1) ? "Pitch" : "Value";

                float stopWidth = 110f;
                var stopRect = new Rect(line2.xMax - stopWidth, line2.y, stopWidth, line2.height);

                // available space left of stop toggle
                float avail = stopRect.x - Pad - line2.x;

                // We remove any forced minimums here
                float labelW = 62f;
                float valueBlockW = Mathf.Max(0f, (avail - Pad) * 0.5f);
                float durBlockW = Mathf.Max(0f, avail - Pad - valueBlockW);

                // Value field
                var valueLabelRect = new Rect(line2.x, line2.y, labelW, line2.height);
                var valueFieldRect = new Rect(valueLabelRect.xMax,
                                              line2.y,
                                              Mathf.Max(0f, valueBlockW - labelW),
                                              line2.height);

                // Duration field
                var durLabelRect = new Rect(valueFieldRect.xMax + Pad, line2.y, labelW, line2.height);
                var durFieldRect = new Rect(durLabelRect.xMax,
                                            line2.y,
                                            Mathf.Max(0f, durBlockW - labelW),
                                            line2.height);

                // Draw
                EditorGUI.LabelField(valueLabelRect, valueLabel);
                EditorGUI.PropertyField(valueFieldRect, targetProp, GUIContent.none);

                EditorGUI.LabelField(durLabelRect, "Duration");
                EditorGUI.PropertyField(durFieldRect, durationProp, GUIContent.none);

                // Right-aligned Stop on end
                stopProp.boolValue = EditorGUI.ToggleLeft(stopRect, "Stop on end", stopProp.boolValue);
            }

            EditorGUI.indentLevel = oldIndent;
            EditorGUI.EndProperty();
        }

        private static bool IsDisable(int enumIndex)
        {
            // expected: 0=Volume, 1=Pitch, 2=Disable
            return enumIndex == 2;
        }
    }
}
