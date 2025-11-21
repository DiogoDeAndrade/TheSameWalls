using UnityEditor;
using UnityEngine;
using UC.Interaction.Editor;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_Wait))]
    public class GameAction_WaitDrawer : BaseGameActionDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Split into content + wait columns
            SplitForWait(position, out var contentRect, out var waitRect);

            var waitTimeProp = property.FindPropertyRelative("waitTime");
            var waitProp = property.FindPropertyRelative("wait");

            // Left side: label + float field
            float labelWidth = EditorGUIUtility.labelWidth;
            var labelRect = new Rect(contentRect.x, contentRect.y, labelWidth, contentRect.height);
            var fieldRect = new Rect(labelRect.xMax, contentRect.y,
                                     contentRect.width - labelWidth, contentRect.height);

            EditorGUI.LabelField(labelRect, "Wait");
            EditorGUI.PropertyField(fieldRect, waitTimeProp, GUIContent.none);

            // Right side: forced and disabled "wait" checkbox
            if (waitProp != null)
            {
                waitProp.boolValue = true;
                using (new EditorGUI.DisabledScope(true))
                {
                    DrawWait(waitRect, property);
                }
            }
        }
    }
}
