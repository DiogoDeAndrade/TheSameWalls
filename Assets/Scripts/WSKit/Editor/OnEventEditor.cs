// Assets/Editor/OnEventEditor.cs
using System;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace WSKit.Editor
{

    [CustomEditor(typeof(OnEvent))]
    public class OnEventEditor : UnityEditor.Editor
    {
        SerializedProperty _conditionsProp;
        SerializedProperty _actionsProp;

        ReorderableList _conditionsList;
        ReorderableList _actionsList;

        void OnEnable()
        {
            _conditionsProp = serializedObject.FindProperty("conditions");
            _actionsProp = serializedObject.FindProperty("actions");

            _conditionsList = BuildManagedReferenceList(
                _conditionsProp,
                typeof(WSKit.Condition),
                "Conditions",
                "Add Condition",
                "No Conditions"
            );

            _actionsList = BuildManagedReferenceList(
                _actionsProp,
                typeof(WSKit.GameAction),
                "Actions",
                "Add Action",
                "No Actions"
            );
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.Space(4);
            _conditionsList.DoLayoutList();

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

            // Example: "Assembly-CSharp WSKit.Condition_HasItem"
            var full = element.managedReferenceFullTypename;

            // Extract the "TypeFullName" part (right side of the space)
            int lastSpace = full.LastIndexOf(' ');
            string typeFullName = (lastSpace >= 0 && lastSpace < full.Length - 1)
                ? full.Substring(lastSpace + 1)
                : full;

            // Normalize nested types (A+B -> A.B), then strip namespace (WSKit.Condition_X -> Condition_X)
            typeFullName = typeFullName.Replace('+', '.');
            int lastDot = typeFullName.LastIndexOf('.');
            string shortName = (lastDot >= 0) ? typeFullName.Substring(lastDot + 1) : typeFullName;

            // Remove a leading "<BaseType>_" prefix irrespective of namespace.
            // This works for Condition_*, GameAction_*, Trigger_*, etc., even if the class lives in WSKit.*
            string basePrefix = baseType.Name + "_";
            if (shortName.StartsWith(basePrefix, StringComparison.Ordinal))
                shortName = shortName.Substring(basePrefix.Length);
            else
            {
                // Also handle some legacy/hardcoded prefixes if you mix bases.
                if (shortName.StartsWith("Condition_", StringComparison.Ordinal))
                    shortName = shortName.Substring("Condition_".Length);
                else if (shortName.StartsWith("GameAction_", StringComparison.Ordinal))
                    shortName = shortName.Substring("GameAction_".Length);
            }

            return ObjectNames.NicifyVariableName(shortName);
        }

    }
}