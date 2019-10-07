using System.IO;
using System.Linq;

using UnityEditor;

using UnityEngine;

namespace Assets.Scripts.Editor
{
    public static class ShowExtension
    {
        private static GUIStyle _activeStyle;
        private static GUIStyle _normalStyle;

        [InitializeOnLoadMethod]
        private static void OnLoad()
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

            var filename = Path.GetFileNameWithoutExtension(path);
            var label = EditorStyles.label;
            var vector = label.CalcSize(new GUIContent(filename));

            selectionRect.x += vector.x + 12;
            selectionRect.y += 2;

            if (_activeStyle == null)
                _activeStyle = new GUIStyle { normal = new GUIStyleState { textColor = Color.white } };
            if (_normalStyle == null)
                _normalStyle = new GUIStyle { normal = new GUIStyleState { textColor = Color.black } };

            GUI.Label(selectionRect, extension, IsSelectedInProjectView(path) ? _activeStyle : _normalStyle);
        }

        private static bool IsSelectedInProjectView(string path)
        {
            var selections = Selection.GetFiltered<Object>(SelectionMode.Assets);
            return selections.Any(w => AssetDatabase.GetAssetPath(w.GetInstanceID()) == path);
        }
    }
}