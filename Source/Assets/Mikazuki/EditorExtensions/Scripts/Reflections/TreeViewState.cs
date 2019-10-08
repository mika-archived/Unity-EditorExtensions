using System.Collections.Generic;
using System.Reflection;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class TreeViewState : ReflectionAccessor<UnityEditor.IMGUI.Controls.TreeViewState>
    {
        private readonly RenameOverlay _renameOverlay;

        public List<int> SelectedIds
        {
            get { return Instance.selectedIDs; }
        }

        public List<int> ExpandedIds
        {
            get { return Instance.expandedIDs; }
        }

        public TreeViewState(UnityEditor.IMGUI.Controls.TreeViewState instance) : base(instance)
        {
            _renameOverlay = new RenameOverlay(AccessProperty<object>("renameOverlay", BindingFlags.Instance | BindingFlags.NonPublic));
        }

        public bool IsRenaming(int instanceId)
        {
            return _renameOverlay.IsRenaming() && _renameOverlay.UserData == instanceId;
        }
    }
}