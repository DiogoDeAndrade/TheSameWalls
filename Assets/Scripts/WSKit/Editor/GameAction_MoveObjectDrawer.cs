using UnityEditor;
using UnityEngine;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_MoveObject))]
    public class GameAction_MoveObjectDrawer : BaseGameActionDrawer
    {
        static readonly float Line = EditorGUIUtility.singleLineHeight;
        const float Pad = 4f;
        const float WaitW = 20f;
        const float DurW = 70f; // duration field width

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return Line; // single line
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var objProp = property.FindPropertyRelative("objectToMove");
            var tgtProp = property.FindPropertyRelative("targetTransform");
            var durProp = property.FindPropertyRelative("duration");
            var waitProp = property.FindPropertyRelative("wait"); // may be null if not in base

            // Reserve space for right-aligned wait checkbox (keep alignment consistent)
            Rect row = position;
            Rect waitRect = new Rect(row.xMax - WaitW, row.y, WaitW, Line);
            float rightEdge = waitRect.x - Pad;

            // Label sizes
            var style = EditorStyles.miniLabel;
            float wMove = style.CalcSize(new GUIContent("Move")).x;
            float wTo = style.CalcSize(new GUIContent("to")).x;
            float wIn = style.CalcSize(new GUIContent("in")).x;
            float wSec = style.CalcSize(new GUIContent("sec")).x;

            // Fixed parts width (labels + duration + paddings between chunks)
            float fixedW = wMove + wTo + wIn + wSec + DurW + (Pad * 6);
            float flexibleW = Mathf.Max(0f, rightEdge - row.x - fixedW);
            // Split flexible area between the two object fields
            float objW = Mathf.Max(90f, flexibleW * 0.5f);
            float tgtW = Mathf.Max(90f, flexibleW - objW);

            // Build rects in sequence
            float x = row.x;

            var rMoveLbl = new Rect(x, row.y, wMove, Line); x = rMoveLbl.xMax + Pad;
            var rObj = new Rect(x, row.y, objW, Line); x = rObj.xMax + Pad;
            var rToLbl = new Rect(x, row.y, wTo, Line); x = rToLbl.xMax + Pad;
            var rTgt = new Rect(x, row.y, tgtW, Line); x = rTgt.xMax + Pad;
            var rInLbl = new Rect(x, row.y, wIn, Line); x = rInLbl.xMax + Pad;
            var rDur = new Rect(x, row.y, DurW, Line); x = rDur.xMax + Pad;
            var rSecLbl = new Rect(x, row.y, wSec, Line);

            EditorGUI.BeginProperty(position, label, property);

            // Draw
            EditorGUI.LabelField(rMoveLbl, "Move", style);
            if (objProp != null) EditorGUI.PropertyField(rObj, objProp, GUIContent.none);

            EditorGUI.LabelField(rToLbl, "to", style);
            if (tgtProp != null) EditorGUI.PropertyField(rTgt, tgtProp, GUIContent.none);

            EditorGUI.LabelField(rInLbl, "in", style);
            if (durProp != null) EditorGUI.PropertyField(rDur, durProp, GUIContent.none);

            EditorGUI.LabelField(rSecLbl, "sec", style);

            if (waitProp != null)
                EditorGUI.PropertyField(waitRect, waitProp, GUIContent.none);

            EditorGUI.EndProperty();
        }
    }
}
