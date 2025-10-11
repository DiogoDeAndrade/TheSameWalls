// Editor/Condition_NumericDrawer.cs
using UnityEditor;
using UnityEngine;
using WSKit;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(Condition_Numeric))]
    public class ConditionNumericDrawer : PropertyDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // We want two rows tall (TYPE/DATA), same as ValueBase drawer
            var line = EditorGUIUtility.singleLineHeight;
            var vspc = EditorGUIUtility.standardVerticalSpacing;
            return line * 2f + vspc * 1f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Layout columns: [Value1 block] [op] [Value2 block]
            // Keep a small gap between blocks
            float gap = 6f;
            float opWidth = 80f;       // “Cond” popup width
            float totalGap = gap * 2f + opWidth;
            float colWidth = (position.width - totalGap) * 0.5f;

            var leftRect = new Rect(position.x, position.y, colWidth, position.height);
            var opRect = new Rect(position.x + colWidth + gap, position.y, opWidth, EditorGUIUtility.singleLineHeight);
            var rightRect = new Rect(opRect.xMax + gap, position.y, colWidth, position.height);

            // Child properties
            var opProp = property.FindPropertyRelative("op");
            var value1Prop = property.FindPropertyRelative("value1");
            var value2Prop = property.FindPropertyRelative("value2");

            // Draw left ValueBase (TYPE + DATA)
            EditorGUI.PropertyField(leftRect, value1Prop, GUIContent.none, true);

            // Draw center op popup (with tiny “Cond” label look)
            var labelRect = new Rect(opRect.x, opRect.y, opRect.width, opRect.height);
            EditorGUI.BeginChangeCheck();
            // Draw mini bold label above? Keep it inline to match your sketch
            EditorGUI.PropertyField(opRect, opProp, GUIContent.none);
            if (EditorGUI.EndChangeCheck())
            {
                opProp.serializedObject.ApplyModifiedProperties();
            }

            // Draw right ValueBase (TYPE + DATA)
            EditorGUI.PropertyField(rightRect, value2Prop, GUIContent.none, true);
        }
    }
}
