// Assets/Editor/InteractableEditor.cs
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace WSKit.Editor
{

    [CustomEditor(typeof(Interactable), true)]
    public class InteractableEditor : UnityEditor.Editor
    {
        // Serialized fields (names taken from Interactable.cs)
        SerializedProperty _interactionVerb;
        SerializedProperty _priority;
        SerializedProperty _overrideCursor;
        SerializedProperty _cursorDef;

        SerializedProperty _conditions;       // [SerializeReference] WSKit.Condition[]
        SerializedProperty _actions;
        SerializedProperty _cooldown;
        SerializedProperty _canRetrigger;

        ReorderableList _conditionsList;
        ReorderableList _actionsList;

        void OnEnable()
        {
            // Top block (matches WSKit.Interactable fields) :contentReference[oaicite:0]{index=0}
            _interactionVerb = serializedObject.FindProperty("interactionVerb");
            _priority = serializedObject.FindProperty("_priority");
            _overrideCursor = serializedObject.FindProperty("overrideCursor");
            _cursorDef = serializedObject.FindProperty("cursorDef");

            // Action block (matches WSKit.Interactable fields) :contentReference[oaicite:1]{index=1}
            _conditions = serializedObject.FindProperty("conditions");
            _actions = serializedObject.FindProperty("actions");
            _cooldown = serializedObject.FindProperty("cooldown");
            _canRetrigger = serializedObject.FindProperty("canRetrigger");

            _conditionsList = BuildManagedReferenceList(
                _conditions, typeof(Condition), "Conditions", "Add Condition", "No conditions");

            _actionsList = BuildManagedReferenceList(
                _actions, typeof(GameAction), "Actions", "Add Action", "No actions");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            
            EditorGUILayout.LabelField("Setup", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            EditorGUILayout.PropertyField(_interactionVerb, new GUIContent("Interaction Verb"));
            EditorGUILayout.PropertyField(_priority, new GUIContent("Priority"));
            EditorGUILayout.PropertyField(_overrideCursor, new GUIContent("Override Cursor"));
            if (_overrideCursor.boolValue)
            {
                EditorGUILayout.PropertyField(_cursorDef, new GUIContent("Cursor"));
            }

            EditorGUI.indentLevel--;

            EditorGUILayout.Space(8);

            // Action 
            EditorGUILayout.LabelField("Action", EditorStyles.boldLabel);
            EditorGUI.indentLevel++;

            _conditionsList.DoLayoutList();
            _actionsList.DoLayoutList();

            EditorGUILayout.PropertyField(_cooldown, new GUIContent("Cooldown"));
            EditorGUILayout.PropertyField(_canRetrigger, new GUIContent("Can Retrigger"));

            EditorGUI.indentLevel--;

            serializedObject.ApplyModifiedProperties();
        }

        // Reorderable managed-reference list (same UX as OnEvent)
        ReorderableList BuildManagedReferenceList(
            SerializedProperty arrayProp,
            Type baseType,
            string header,
            string addLabel,
            string emptyLabel)
        {
            var list = new ReorderableList(serializedObject, arrayProp, true, true, true, true);

            list.drawHeaderCallback = rect =>
            {
                EditorGUI.LabelField(rect, header, EditorStyles.boldLabel);
            };

            list.elementHeightCallback = index =>
            {
                var el = arrayProp.GetArrayElementAtIndex(index);
                return Mathf.Max(
                    EditorGUI.GetPropertyHeight(el, includeChildren: true) + 4f,
                    EditorGUIUtility.singleLineHeight + 6f
                );
            };

            list.drawElementCallback = (rect, index, isActive, isFocused) =>
            {
                var el = arrayProp.GetArrayElementAtIndex(index);
                rect.height = EditorGUI.GetPropertyHeight(el, includeChildren: true);
                EditorGUI.PropertyField(rect, el, new GUIContent(GetManagedLabel(el, baseType)), includeChildren: true);
            };

            list.onAddDropdownCallback = (buttonRect, l) =>
            {
                ManagedReferenceAddMenu.Show(buttonRect, baseType, instance =>
                {
                    ManagedReferenceAddMenu.InsertNewManagedElement(arrayProp, arrayProp.arraySize, instance);
                });
            };

            list.onRemoveCallback = l =>
            {
                if (l.index >= 0 && l.index < arrayProp.arraySize)
                {
                    var el = arrayProp.GetArrayElementAtIndex(l.index);
                    el.managedReferenceValue = null;
                    arrayProp.DeleteArrayElementAtIndex(l.index);
                    serializedObject.ApplyModifiedProperties();
                }
            };

            list.drawNoneElementCallback = rect =>
            {
                EditorGUI.LabelField(rect, emptyLabel, EditorStyles.miniLabel);
            };

            // Right-click -> Replace type
            list.onMouseUpCallback = l =>
            {
                if (Event.current != null && Event.current.button == 1 && l.index >= 0)
                {
                    var index = l.index;
                    var menu = new GenericMenu();
                    menu.AddItem(new GUIContent("Replace..."), false, () =>
                    {
                        var last = GUILayoutUtility.GetLastRect();
                        ManagedReferenceAddMenu.Show(last, baseType, instance =>
                        {
                            var el = arrayProp.GetArrayElementAtIndex(index);
                            el.managedReferenceValue = instance;
                            serializedObject.ApplyModifiedProperties();
                        });
                    });
                    menu.ShowAsContext();
                }
            };

            return list;
        }

        // Same robust label clean-up you asked for previously
        static string GetManagedLabel(SerializedProperty element, Type baseType)
        {
            if (element.managedReferenceValue == null)
                return $"({baseType.Name}) None";

            var full = element.managedReferenceFullTypename; // "Assembly TypeFullName"
            int lastSpace = full.LastIndexOf(' ');
            string typeFullName = (lastSpace >= 0 && lastSpace < full.Length - 1)
                ? full.Substring(lastSpace + 1)
                : full;

            typeFullName = typeFullName.Replace('+', '.'); // nested types
            int lastDot = typeFullName.LastIndexOf('.');
            string shortName = (lastDot >= 0) ? typeFullName[(lastDot + 1)..] : typeFullName;

            string basePrefix = baseType.Name + "_";
            if (shortName.StartsWith(basePrefix, StringComparison.Ordinal))
                shortName = shortName.Substring(basePrefix.Length);
            else
            {
                if (shortName.StartsWith("Condition_", StringComparison.Ordinal))
                    shortName = shortName.Substring("Condition_".Length);
                else if (shortName.StartsWith("Trigger_", StringComparison.Ordinal))
                    shortName = shortName.Substring("Trigger_".Length);
                else if (shortName.StartsWith("GameAction_", StringComparison.Ordinal))
                    shortName = shortName.Substring("GameAction_".Length);
            }

            return ObjectNames.NicifyVariableName(shortName);
        }
    }
}