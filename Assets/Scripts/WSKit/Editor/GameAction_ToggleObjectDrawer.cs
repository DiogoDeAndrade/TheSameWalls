using UnityEditor;
using UnityEngine;
using UC.Interaction.Editor;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_ToggleObject))]
    public class GameAction_ToggleObjectDrawer : BaseGameActionDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var targetProp = property.FindPropertyRelative("targetGameObject");
            var stateProp = property.FindPropertyRelative("state");

            float totalWidth = position.width;
            float labelWidth = EditorGUIUtility.labelWidth;
            float fieldWidth = (totalWidth - labelWidth) * 0.6f;
            float enumWidth = (totalWidth - labelWidth) * 0.4f;

            var labelRect = new Rect(position.x, position.y, labelWidth, position.height);
            var targetRect = new Rect(position.x + labelWidth, position.y, fieldWidth - 4, position.height);
            var enumRect = new Rect(targetRect.xMax + 4, position.y, enumWidth - 4, position.height);

            EditorGUI.LabelField(labelRect, "Toggle Object");
            EditorGUI.PropertyField(targetRect, targetProp, GUIContent.none);
            EditorGUI.PropertyField(enumRect, stateProp, GUIContent.none);
        }
    }
}
