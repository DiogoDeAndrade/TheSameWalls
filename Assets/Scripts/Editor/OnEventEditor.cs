// Assets/Editor/OnEventEditor.cs
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

[CustomEditor(typeof(OnEvent))]
public class OnEventEditor : Editor
{
    SerializedProperty _triggersProp;
    SerializedProperty _actionsProp;

    ReorderableList _triggersList;
    ReorderableList _actionsList;

    void OnEnable()
    {
        _triggersProp = serializedObject.FindProperty("triggers");
        _actionsProp = serializedObject.FindProperty("actions");

        _triggersList = BuildManagedReferenceList(
            _triggersProp,
            typeof(Trigger),
            "Triggers",
            "Add Trigger",
            "No Triggers"
        );

        _actionsList = BuildManagedReferenceList(
            _actionsProp,
            typeof(GameAction),
            "Actions",
            "Add Action",
            "No Actions"
        );
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.Space(4);
        _triggersList.DoLayoutList();

        EditorGUILayout.Space(8);
        _actionsList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
    }

    private ReorderableList BuildManagedReferenceList(
        SerializedProperty arrayProp,
        Type baseType,
        string header,
        string addButtonLabel,
        string emptyLabel)
    {
        var list = new ReorderableList(serializedObject, arrayProp, true, true, true, true);

        list.drawHeaderCallback = rect =>
        {
            EditorGUI.LabelField(rect, header, EditorStyles.boldLabel);
        };

        list.elementHeight = EditorGUIUtility.singleLineHeight * 1.2f; // base height, will expand to property height

        list.drawElementCallback = (rect, index, isActive, isFocused) =>
        {
            var element = arrayProp.GetArrayElementAtIndex(index);
            // Managed references can expand (so we rely on PropertyField to compute height properly)
            rect.height = EditorGUI.GetPropertyHeight(element, includeChildren: true);
            EditorGUI.PropertyField(rect, element, new GUIContent(GetManagedLabel(element, baseType)), includeChildren: true);
        };

        list.elementHeightCallback = index =>
        {
            var element = arrayProp.GetArrayElementAtIndex(index);
            return Mathf.Max(EditorGUI.GetPropertyHeight(element, includeChildren: true) + 4f, EditorGUIUtility.singleLineHeight + 6f);
        };

        // Add button: choose subclass
        list.onAddDropdownCallback = (buttonRect, l) =>
        {
            ManagedReferenceAddMenu.Show(buttonRect, baseType, instance =>
            {
                ManagedReferenceAddMenu.InsertNewManagedElement(arrayProp, arrayProp.arraySize, instance);
            });
        };

        // Remove button: clear managed reference safely
        list.onRemoveCallback = l =>
        {
            if (l.index >= 0 && l.index < arrayProp.arraySize)
            {
                var element = arrayProp.GetArrayElementAtIndex(l.index);
                // Explicitly null the managed reference, then delete array slot
                element.managedReferenceValue = null;
                arrayProp.DeleteArrayElementAtIndex(l.index);
                serializedObject.ApplyModifiedProperties();
            }
        };

        list.drawNoneElementCallback = rect =>
        {
            EditorGUI.LabelField(rect, emptyLabel, EditorStyles.miniLabel);
        };

        // Context: replace element type
        list.onMouseUpCallback = l =>
        {
            if (Event.current != null && Event.current.button == 1 && l.index >= 0)
            {
                var index = l.index;
                var menu = new GenericMenu();
                menu.AddItem(new GUIContent("Replace…"), false, () =>
                {
                    var elemRect = GUILayoutUtility.GetLastRect();
                    ManagedReferenceAddMenu.Show(elemRect, baseType, instance =>
                    {
                        var element = arrayProp.GetArrayElementAtIndex(index);
                        element.managedReferenceValue = instance;
                        serializedObject.ApplyModifiedProperties();
                    });
                });
                menu.ShowAsContext();
            }
        };

        return list;
    }

    private static string GetManagedLabel(SerializedProperty element, Type baseType)
    {
        if (element.managedReferenceValue == null)
            return $"({baseType.Name}) None";

        var full = element.managedReferenceFullTypename; // "AssemblyName TypeName"
        var lastSpace = full.LastIndexOf(' ');
        var typeName = (lastSpace >= 0 && lastSpace < full.Length - 1) ? full[(lastSpace + 1)..] : full;

        if (baseType == typeof(Trigger) && typeName.StartsWith("Trigger_"))
            typeName = typeName.Substring("Trigger_".Length);

        if (baseType == typeof(GameAction) && typeName.StartsWith("GameAction_"))
            typeName = typeName.Substring("GameAction_".Length);

        return ObjectNames.NicifyVariableName(typeName);
    }

}
