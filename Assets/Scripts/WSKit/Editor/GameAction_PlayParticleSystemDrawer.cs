using UnityEditor;
using UnityEngine;
using UC.Interaction.Editor;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(GameAction_PlayParticleSystem))]
    public class GameAction_PlayParticleSystemDrawer : BaseGameActionDrawer
    {
        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            return EditorGUIUtility.singleLineHeight;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            // Split into content + wait columns (from your BaseGameActionDrawer)
            SplitForWait(position, out var contentRect, out var waitRect);

            // Left content: "Play" label + ParticleSystem selector
            var psProp = property.FindPropertyRelative("particleSystem");

            float labelWidth = EditorGUIUtility.labelWidth;
            var labelRect = new Rect(contentRect.x, contentRect.y, labelWidth, contentRect.height);
            var fieldRect = new Rect(labelRect.xMax, contentRect.y,
                                     contentRect.width - labelWidth, contentRect.height);

            EditorGUI.LabelField(labelRect, "Play Particle System");
            EditorGUI.PropertyField(fieldRect, psProp, GUIContent.none);

            // Right content: the standardized wait toggle (no label)
            DrawWait(waitRect, property);
        }
    }
}
