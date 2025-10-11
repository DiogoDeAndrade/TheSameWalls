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
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var stateProp = property.FindPropertyRelative("state");

            // Split into content and wait columns (from BaseGameActionDrawer)
            SplitForWait(position, out var content, out var waitRect);

            var style = EditorStyles.miniLabel;
            float lineH = EditorGUIUtility.singleLineHeight;
            float pad = 4f;

            // Measure text widths
            float wToggle = style.CalcSize(new GUIContent("Toggle Input")).x;
            float enumW = 80f; // enough for On/Off/Toggle

            float x = content.x;

            // Layout
            var rToggleLbl = new Rect(x, content.y, wToggle, lineH); x = rToggleLbl.xMax + pad;
            var rEnum = new Rect(x, content.y, enumW, lineH);

            EditorGUI.BeginProperty(position, label, property);

            // Draw fields
            EditorGUI.LabelField(rToggleLbl, "Toggle Input", style);
            EditorGUI.PropertyField(rEnum, stateProp, GUIContent.none);

            DrawWait(waitRect, property);

            EditorGUI.EndProperty();
        }
    }
}
