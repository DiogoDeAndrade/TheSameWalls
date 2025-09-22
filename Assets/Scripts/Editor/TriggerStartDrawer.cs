// Assets/Editor/TriggerStartDrawer.cs
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(Trigger_Start))]
public class TriggerStartDrawer : PropertyDrawer
{
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        => EditorGUIUtility.singleLineHeight;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        => EditorGUI.LabelField(position, label);
}
