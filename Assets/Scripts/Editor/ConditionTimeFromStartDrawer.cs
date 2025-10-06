// Assets/Editor/ConditionTimeFromStartDrawer.cs
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Condition_TimeFromStart))]
public class ConditionTimeFromStartDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => EditorGUIUtility.singleLineHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        var negateProp = property.FindPropertyRelative("negate");
        var timeProp = property.FindPropertyRelative("elapsedTime");

        const float notW = 48f;
        var notRect = new Rect(position.x, position.y, notW, position.height);
        var restRect = new Rect(position.x + notW, position.y, position.width - notW, position.height);

        negateProp.boolValue = EditorGUI.ToggleLeft(notRect, "Not", negateProp.boolValue);

        float labelWidth = Mathf.Max(50f, EditorGUIUtility.labelWidth - notW);
        var labelRect = new Rect(restRect.x, restRect.y, labelWidth, restRect.height);
        var fieldRect = new Rect(restRect.x + labelWidth, restRect.y, restRect.width - labelWidth, restRect.height);

        // Label is whatever your list provides (e.g., "Time From Start")
        EditorGUI.LabelField(labelRect, label);
        EditorGUI.PropertyField(fieldRect, timeProp, GUIContent.none);
    }
}
