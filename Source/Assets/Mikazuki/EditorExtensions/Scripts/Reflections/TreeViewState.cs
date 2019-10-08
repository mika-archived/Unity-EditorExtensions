using System.Collections.Generic;
using System.Reflection;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class TreeViewState : ReflectionAccessor<UnityEditor.IMGUI.Controls.TreeViewState>
    {
        public List<int> SelectedIds
        {
            get { return Instance.selectedIDs; }
        }

        public List<int> ExpandedIds
        {
            get { return Instance.expandedIDs; }
        }

        public TreeViewState(UnityEditor.IMGUI.Controls.TreeViewState instance) : base(instance) { }
    }
}