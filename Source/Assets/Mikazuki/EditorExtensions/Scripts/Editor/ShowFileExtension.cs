using System.IO;
using System.Linq;

using Assets.Mikazuki.EditorExtensions.Scripts.Reflections;

using UnityEditor;

using UnityEngine;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Editor
{
    [InitializeOnLoad]
    public static class ShowFileExtension
    {
        private static GUIStyle _activeStyle;
        private static GUIStyle _normalStyle;
        private static ProjectBrowser _browser;

        static ShowFileExtension()
        {
            // ReSharper disable once DelegateSubtraction
            EditorApplication.projectWindowItemOnGUI -= ProjectWindowItemOnGui;
            EditorApplication.projectWindowItemOnGUI += ProjectWindowItemOnGui;
        }

        private static void ProjectWindowItemOnGui(string guid, Rect selectionRect)
        {
            var path = AssetDatabase.GUIDToAssetPath(guid);

            if (string.IsNullOrEmpty(path))
                return;

            var extension = Path.GetExtension(path);
            if (string.IsNullOrEmpty(extension))
                return;

            if (IsTwoColumns())
                ShowExtensionOnTwoColumns(selectionRect, path, extension);
            else
                ShowExtensionOnOneColumn(selectionRect, path, extension);
        }

        private static void ShowExtensionOnOneColumn(Rect rect, string path, string extension)
        {
            var filename = Path.GetFileNameWithoutExtension(path);
            var label = EditorStyles.label;
            var vector = label.CalcSize(new GUIContent(filename));

            rect.x += vector.x + 14;
            rect.y += 2;

            ShowLabel(rect, path, extension);
        }

        private static void ShowExtensionOnTwoColumns(Rect rect, string path, string extension)
        {
            var isSingleLine = rect.height <= 20;
            if (isSingleLine)
            {
                var filename = Path.GetFileNameWithoutExtension(path);
                var label = EditorStyles.label;
                var vector = label.CalcSize(new GUIContent(filename));

                rect.x += vector.x + 16;
                rect.y += 2;

                ShowLabel(rect, path, extension);
            }
        }

        private static void ShowLabel(Rect rect, string path, string extension)
        {
            if (_activeStyle == null)
                _activeStyle = new GUIStyle { normal = new GUIStyleState { textColor = Color.white } };
            if (_normalStyle == null)
                _normalStyle = new GUIStyle { normal = new GUIStyleState { textColor = Color.black } };

            GUI.Label(rect, extension, IsSelectedInProjectView(path) ? _activeStyle : _normalStyle);
        }

        private static bool IsSelectedInProjectView(string path)
        {
            var selections = Selection.GetFiltered<Object>(SelectionMode.Assets);
            return selections.Any(w => AssetDatabase.GetAssetPath(w.GetInstanceID()) == path);
        }

        private static bool IsTwoColumns()
        {
            if (_browser == null)
                _browser = ProjectBrowser.Create();
            return _browser.IsTwoColumns();
        }
    }
}