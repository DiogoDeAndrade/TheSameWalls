// Assets/Editor/TriggerTagDrawer.cs
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Trigger_Tag))]
public class TriggerTagDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        // Single line only
        return EditorGUIUtility.singleLineHeight;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var tagProp = property.FindPropertyRelative("tag");

        // Split rect into label + field
        float labelWidth = EditorGUIUtility.labelWidth; // default label width
        var labelRect = new Rect(position.x, position.y, labelWidth, position.height);
        var fieldRect = new Rect(position.x + labelWidth, position.y,
                                 position.width - labelWidth, position.height);

        EditorGUI.LabelField(labelRect, label); // "Tag"
        EditorGUI.PropertyField(fieldRect, tagProp, GUIContent.none);
    }
}
