using System;
using System.IO;

using Assets.Mikazuki.EditorExtensions.Scripts.Reflections;

using UnityEditor;

using UnityEngine;

using Object = UnityEngine.Object;

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

            var instanceId = AssetDatabase.LoadAssetAtPath<Object>(path).GetInstanceID();

            if (IsTwoColumns())
                ShowExtensionOnTwoColumns(selectionRect, path, extension, instanceId);
            else
                ShowExtensionOnOneColumn(selectionRect, path, extension, instanceId);
        }

        private static void ShowExtensionOnOneColumn(Rect rect, string path, string extension, int instanceId)
        {
            if (_browser.AssetTreeState.IsRenaming(instanceId))
                return;
            if (_browser.AssetTreeState.ExpandedIds.Contains(instanceId))
            {
                var controller = _browser.AssetTree;
                var row = controller.GetRowIndex(instanceId);
                var rectForRow = controller.GetRectForRows(row, row, rect.width);

                if (Math.Abs(rect.y - rectForRow.y) > 0)
                    return;
            }

            var filename = Path.GetFileNameWithoutExtension(path);
            var label = EditorStyles.label;
            var vector = label.CalcSize(new GUIContent(filename));

            rect.x += vector.x + 14;
            rect.y += 2;

            ShowLabel(rect, extension, _browser.AssetTreeState.SelectedIds.Contains(instanceId));
        }

        private static void ShowExtensionOnTwoColumns(Rect rect, string path, string extension, int instanceId)
        {
            var isMultiLine = rect.height > 20;
            if (isMultiLine)
                return;
            if (_browser.ListAreaState.IsRenaming(instanceId))
                return;
            if (_browser.ListAreaState.ExpandedInstanceIds.Contains(instanceId))
            {
                var controller = _browser.ListArea;
                var index = controller.LocalAssets.IndexOf(instanceId);
                var rectForGrid = controller.LocalAssets.CalcRect(index);

                if (Math.Abs(rect.y - rectForGrid.y) > 0)
                    return;
            }

            var filename = Path.GetFileNameWithoutExtension(path);
            var label = EditorStyles.label;
            var vector = label.CalcSize(new GUIContent(filename));

            rect.x += vector.x + 16;
            rect.y += 2;

            ShowLabel(rect, extension, _browser.ListAreaState.SelectedInstanceIds.Contains(instanceId));
        }

        private static void ShowLabel(Rect rect, string extension, bool isActive)
        {
            if (_activeStyle == null)
                _activeStyle = new GUIStyle { normal = new GUIStyleState { textColor = Color.white } };
            if (_normalStyle == null)
                _normalStyle = new GUIStyle { normal = new GUIStyleState { textColor = Color.black } };

            GUI.Label(rect, extension, isActive ? _activeStyle : _normalStyle);
        }

        private static bool IsTwoColumns()
        {
            if (_browser == null)
                _browser = ProjectBrowser.Create();
            return _browser.IsTwoColumns();
        }
    }
}