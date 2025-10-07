// Editor/ValueBaseDrawer.cs
using System;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using WSKit;

namespace WSKit.Editor
{
    [CustomPropertyDrawer(typeof(ValueBase), true)]
    public class ValueBaseDrawer : PropertyDrawer
    {
        static readonly Type BaseType = typeof(ValueBase);
        static Type[] _cachedTypes;
        static string[] _cachedNiceNames;

        static void EnsureTypes()
        {
            if (_cachedTypes != null) return;

            _cachedTypes = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(a =>
                {
                    try { return a.GetTypes(); }
                    catch (ReflectionTypeLoadException e) { return e.Types.Where(t => t != null); }
                })
                .Where(t => BaseType.IsAssignableFrom(t) && !t.IsAbstract)
                .OrderBy(t => t.Name)
                .ToArray();

            _cachedNiceNames = _cachedTypes
                .Select(NiceNameForType)
                .ToArray();
        }

        static string NiceNameForType(Type t)
        {
            // Pretty names (extend as you add more ValueBase types)
            if (t == typeof(ValueLiteral)) return "Literal";
            if (t == typeof(ValueTokenCount)) return "Token Count";
            // Fallback: split PascalCase
            return ObjectNames.NicifyVariableName(t.Name.Replace("Value", ""));
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            // Two rows (TYPE, DATA)
            var line = EditorGUIUtility.singleLineHeight;
            var vspc = EditorGUIUtility.standardVerticalSpacing;
            return line * 2f + vspc * 1f;
        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EnsureTypes();

            // Create a default instance if null
            if (property.managedReferenceValue == null)
                property.managedReferenceValue = Activator.CreateInstance(typeof(ValueLiteral));

            var line = EditorGUIUtility.singleLineHeight;
            var vspc = EditorGUIUtility.standardVerticalSpacing;

            // Rects
            var typeRect = new Rect(position.x, position.y, position.width, line);
            var dataRect = new Rect(position.x, position.y + line + vspc, position.width, line);

            // Determine current type index
            string currentType = property.managedReferenceFullTypename; // e.g. "AssemblyName Namespace.TypeName"
            int idx = -1;
            for (int i = 0; i < _cachedTypes.Length; i++)
            {
                if (property.managedReferenceValue != null &&
                    _cachedTypes[i] == property.managedReferenceValue.GetType())
                {
                    idx = i;
                    break;
                }
            }
            if (idx < 0) idx = 0;

            // TYPE popup
            EditorGUI.BeginChangeCheck();
            int newIdx = EditorGUI.Popup(typeRect, idx, _cachedNiceNames);
            if (EditorGUI.EndChangeCheck() && newIdx != idx)
            {
                // record for Undo (all targets)
                foreach (var t in property.serializedObject.targetObjects)
                    Undo.RecordObject(t, "Change Value Type");

                property.serializedObject.Update();                  // pull new fields
                property.managedReferenceValue = Activator.CreateInstance(_cachedTypes[newIdx]);
                property.serializedObject.ApplyModifiedProperties(); // flush
            }

            // DATA field(s): draw child fields of the concrete type without labels
            if (property.managedReferenceValue != null)
            {
                using (new EditorGUI.IndentLevelScope(0))
                {
                    var iter = property.Copy();
                    var end = property.GetEndProperty();

                    if (iter.NextVisible(true) && !SerializedProperty.EqualContents(iter, end))
                    {
                        // iter is now the first child field of the concrete ValueBase
                        EditorGUI.PropertyField(dataRect, iter, GUIContent.none, true);
                    }
                }
            }
        }
    }
}
