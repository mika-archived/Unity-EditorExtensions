using System.Collections.Generic;

using MUE2.Editor.Reflection.Expressions.Generic;

namespace MUE2.Editor.Reflection
{
    public class TreeViewState : ReflectionClass<UnityEditor.IMGUI.Controls.TreeViewState>
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
            _renameOverlay = new RenameOverlay(InvokeMember<object>("renameOverlay"));
        }

        public bool IsRenaming(int instanceId)
        {
            return _renameOverlay.IsRenaming() && _renameOverlay.UserData == instanceId;
        }
    }
}