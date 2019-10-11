using UnityEditor;

using UnityEngine;

namespace Assets.EditorExtensions.Editor.Controls
{
    internal static class EditorContent
    {
        public static bool Button(string text, params GUILayoutOption[] options)
        {
            return GUILayout.Button(text, options);
        }

        public static bool Checkbox(string label, bool value, params GUILayoutOption[] options)
        {
            return EditorGUILayout.Toggle(new GUIContent(label), value, options);
        }

        public static DisabledGroup DisabledGroup(bool disabled)
        {
            return new DisabledGroup(disabled);
        }

        public static Indent Indent()
        {
            return new Indent();
        }

        public static void Label(string label, params GUILayoutOption[] options)
        {
            EditorGUILayout.LabelField(label, options);
        }

        public static T ObjectPicker<T>(string label, Object field, bool allowSceneObjects, params GUILayoutOption[] options) where T : class
        {
            return EditorGUILayout.ObjectField(new GUIContent(label), field, typeof(T), allowSceneObjects, options) as T;
        }

        public static void Space()
        {
            EditorGUILayout.Space();
        }

        public static string TextField(string label, string text, params GUILayoutOption[] options)
        {
            return EditorGUILayout.TextField(label, text, options);
        }

        #region Object Changed

        public static void BeginObjectChanged()
        {
            EditorGUI.BeginChangeCheck();
        }

        public static bool EndObjectChanged()
        {
            return EditorGUI.EndChangeCheck();
        }

        #endregion
    }
}