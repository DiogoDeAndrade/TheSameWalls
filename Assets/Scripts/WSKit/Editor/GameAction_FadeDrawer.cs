// Assets/Editor/GameAction_FadeDrawer.cs
using UnityEditor;
using UnityEngine;
using UC.Interaction.Editor;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_Fade))]
    public class GameAction_FadeDrawer : BaseGameActionDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var stateProp = property.FindPropertyRelative("state");
            var durProp = property.FindPropertyRelative("fadeDuration");
            var colorProp = property.FindPropertyRelative("fadeColor");

            // Split into content + wait columns (BaseGameActionDrawer)
            SplitForWait(position, out var content, out var waitRect);

            var style = EditorStyles.miniLabel;
            float lineH = EditorGUIUtility.singleLineHeight;
            float pad = 4f;

            // small inline labels
            float wFade = style.CalcSize(new GUIContent("Fade")).x;
            float wIn = style.CalcSize(new GUIContent("in")).x;
            float wSec = style.CalcSize(new GUIContent("sec")).x;

            float enumW = 60f;
            float durW = 70f;
            float colorW = 55f; // compact color field

            float x = content.x;

            var rFadeLbl = new Rect(x, content.y, wFade, lineH); x = rFadeLbl.xMax + pad;
            var rEnum = new Rect(x, content.y, enumW, lineH); x = rEnum.xMax + pad;
            var rInLbl = new Rect(x, content.y, wIn, lineH); x = rInLbl.xMax + pad;
            var rDur = new Rect(x, content.y, durW, lineH); x = rDur.xMax + pad;
            var rSecLbl = new Rect(x, content.y, wSec, lineH); x = rSecLbl.xMax + pad;
            var rColor = new Rect(x, content.y, colorW, lineH);

            EditorGUI.BeginProperty(position, label, property);

            EditorGUI.LabelField(rFadeLbl, "Fade", style);
            EditorGUI.PropertyField(rEnum, stateProp, GUIContent.none);
            EditorGUI.LabelField(rInLbl, "in", style);
            EditorGUI.PropertyField(rDur, durProp, GUIContent.none);
            EditorGUI.LabelField(rSecLbl, "sec", style);
            EditorGUI.PropertyField(rColor, colorProp, GUIContent.none);

            DrawWait(waitRect, property);

            EditorGUI.EndProperty();
        }
    }
}
