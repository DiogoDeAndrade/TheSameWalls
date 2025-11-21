// Assets/Editor/GameAction_MoveObjectMultiDrawer.cs
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UC.Interaction.Editor;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_MoveObjectMulti))]
    public class GameAction_MoveObjectMultiDrawer : BaseGameActionDrawer
    {
        static readonly float Line = EditorGUIUtility.singleLineHeight;
        const float Pad = 4f;
        const float WaitW = 20f;
        const float DurW = 70f;

        private static readonly Dictionary<string, ReorderableList> _lists = new();

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            var list = GetOrCreateList(property);
            // first row + small gap + list height
            return Mathf.Max(Line, list.GetHeight()) + 2.0f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var objProp = property.FindPropertyRelative("objectToMove");
            var durProp = property.FindPropertyRelative("duration");
            var waitProp = property.FindPropertyRelative("wait");

            Rect row = new Rect(position.x, position.y, position.width, Line);
            Rect waitRect = new Rect(row.xMax - WaitW, row.y, WaitW, Line);
            float rightEdge = waitRect.x - Pad;

            var style = EditorStyles.miniLabel;
            float wMove = style.CalcSize(new GUIContent("Move")).x;
            float wTo = style.CalcSize(new GUIContent("to")).x;
            float wIn = style.CalcSize(new GUIContent("in")).x;
            float wSec = style.CalcSize(new GUIContent("sec")).x;

            float fixedW = wMove + wTo + wIn + wSec + DurW + (Pad * 6);
            float flexibleW = Mathf.Max(0f, rightEdge - row.x - fixedW);
            float objW = Mathf.Max(90f, flexibleW * 0.5f);
            float tgtW = Mathf.Max(90f, flexibleW - objW) - 12.0f;

            float x = row.x;

            var rMoveLbl = new Rect(x, row.y, wMove, Line); x = rMoveLbl.xMax + Pad;
            var rObj = new Rect(x, row.y, objW, Line); x = rObj.xMax + Pad;
            var rToLbl = new Rect(x, row.y, wTo, Line); x = rToLbl.xMax + Pad;

            // column where the list sits
            var rTgtCol = new Rect(x + 12.0f, row.y, tgtW, Line); x = rTgtCol.xMax + Pad;

            var rInLbl = new Rect(x, row.y, wIn, Line); x = rInLbl.xMax + Pad;
            var rDur = new Rect(x, row.y, DurW, Line); x = rDur.xMax + Pad;
            var rSecLbl = new Rect(x, row.y, wSec, Line);

            EditorGUI.BeginProperty(position, label, property);

            // first line
            EditorGUI.LabelField(rMoveLbl, "Move", style);
            EditorGUI.PropertyField(rObj, objProp, GUIContent.none);
            EditorGUI.LabelField(rToLbl, "to", style);

            EditorGUI.LabelField(rInLbl, "in", style);
            EditorGUI.PropertyField(rDur, durProp, GUIContent.none);
            EditorGUI.LabelField(rSecLbl, "sec", style);
            if (waitProp != null) EditorGUI.PropertyField(waitRect, waitProp, GUIContent.none);

            // list area (headerless, with + / -)
            float width = rInLbl.xMin - rToLbl.xMax;

            var listRect = new Rect(rTgtCol.x, row.y + 2f, width - 5.0f, 0f);
            var list = GetOrCreateList(property);
            listRect.height = list.GetHeight();
            list.DoList(listRect); // use the list instance directly

            EditorGUI.EndProperty();
        }

        private ReorderableList GetOrCreateList(SerializedProperty actionProperty)
        {
            var tgtProp = actionProperty.FindPropertyRelative("targetTransforms");
            string key = actionProperty.serializedObject.targetObject.GetInstanceID() + "|" + actionProperty.propertyPath;

            if (_lists.TryGetValue(key, out var list) && list.serializedProperty == tgtProp)
                return list;

            list = new ReorderableList(actionProperty.serializedObject, tgtProp,
                                       draggable: true, displayHeader: false,
                                       displayAddButton: true, displayRemoveButton: true);

            list.elementHeight = Line + 2f;
            list.headerHeight = 0f;

            list.drawElementCallback = (Rect r, int index, bool isActive, bool isFocused) =>
            {
                var element = tgtProp.GetArrayElementAtIndex(index);

                const float idxW = 18f;
                var idxRect = new Rect(r.x, r.y + 1f, idxW, Line);
                var fieldRect = new Rect(idxRect.xMax + 2f, r.y, r.width - idxW - 2f, Line);

                EditorGUI.LabelField(idxRect, index.ToString(), EditorStyles.miniLabel); // just the number
                EditorGUI.PropertyField(fieldRect, element, GUIContent.none);            // no “Element”
            };

            list.drawNoneElementCallback = (Rect r) =>
            {
                EditorGUI.LabelField(r, "No targets", EditorStyles.centeredGreyMiniLabel);
            };

            _lists[key] = list;
            return list;
        }
    }
}
