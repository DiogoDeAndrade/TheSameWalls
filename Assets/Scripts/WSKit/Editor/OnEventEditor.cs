using UnityEditor;
using UnityEditorInternal;
using UnityEngine;
using UC.Interaction;
using UC.Interaction.Editor;

namespace WSKit.Editor
{

    [CustomEditor(typeof(OnEvent))]
    public class OnEventEditor : UnityEditor.Editor
    {
        SerializedProperty _eventTypeProp;
        SerializedProperty _referenceObjectProp;
        SerializedProperty _conditionsProp;
        SerializedProperty _actionsProp;
        SerializedProperty _retriggerProp;
        SerializedProperty _cooldownProp;

        ReorderableList _conditionsList;
        ReorderableList _actionsList;

        void OnEnable()
        {
            _eventTypeProp = serializedObject.FindProperty("_eventType");
            _referenceObjectProp = serializedObject.FindProperty("_referenceObject");
            _conditionsProp = serializedObject.FindProperty("conditions");
            _actionsProp = serializedObject.FindProperty("actions");
            _retriggerProp = serializedObject.FindProperty("retrigger");
            _cooldownProp = serializedObject.FindProperty("cooldown");

            _conditionsList = ManagedReferenceListHelper.Build(serializedObject, _conditionsProp, typeof(Condition), "Conditions", "Add Condition", "No Conditions");

            _actionsList = ManagedReferenceListHelper.Build(serializedObject, _actionsProp, typeof(GameAction), "Actions", "Add Action", "No Actions", rightHeader: "Wait", rightHeaderWidth: BaseGameActionDrawer.WaitColumnWidth);
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            EditorGUILayout.PropertyField(_eventTypeProp, new GUIContent("Event Type"));
            EditorGUILayout.PropertyField(_referenceObjectProp, new GUIContent("Reference Object"));
            EditorGUILayout.PropertyField(_retriggerProp, new GUIContent("Retrigger?"));
            if (_retriggerProp.boolValue)
            {
                EditorGUILayout.PropertyField(_cooldownProp, new GUIContent("Cooldown"));
            }

            EditorGUILayout.Space(4);
            _conditionsList.DoLayoutList();

            EditorGUILayout.Space(8);
            _actionsList.DoLayoutList();

            serializedObject.ApplyModifiedProperties();
        }        
    }
}