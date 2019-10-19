using UnityEditor;

using UnityEngine;

namespace MUE2.Editor.Controls
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
            var style = new GUIStyle { wordWrap = true, margin = new RectOffset(5, 5, 0, 0) };
            EditorGUILayout.LabelField(label, style, options);
        }

        public static T ObjectPicker<T>(string label, Object field, bool allowSceneObjects, params GUILayoutOption[] options) where T : class
        {
            return EditorGUILayout.ObjectField(new GUIContent(label), field, typeof(T), allowSceneObjects, options) as T;
        }

        public static int Select(string label, int selectedIndex, params string[] displayedOptions)
        {
            return Select(label, selectedIndex, displayedOptions, new GUILayoutOption[] { });
        }

        public static int Select(string label, int selectedIndex, string[] displayedOptions, params GUILayoutOption[] options)
        {
            return EditorGUILayout.Popup(label, selectedIndex, displayedOptions, options);
        }

        public static void Space()
        {
            EditorGUILayout.Space();
        }

        public static int Tab(int selected, params string[] texts)
        {
            return Tab(selected, texts, new GUILayoutOption[] { });
        }

        public static int Tab(int selected, string[] texts, params GUILayoutOption[] options)
        {
            int newSelected;
            using (new EditorGUILayout.HorizontalScope())
            {
                GUILayout.FlexibleSpace();

                newSelected = GUILayout.Toolbar(selected, texts, options);

                GUILayout.FlexibleSpace();
            }

            return newSelected;
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

        public static bool IsObjectChanged()
        {
            return EditorGUI.EndChangeCheck();
        }

        #endregion
    }
}