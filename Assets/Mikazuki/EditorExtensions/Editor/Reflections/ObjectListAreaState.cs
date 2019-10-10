using System.Collections.Generic;
using System.Reflection;

namespace Assets.Mikazuki.EditorExtensions.Editor.Reflections
{
    public class ObjectListAreaState : ReflectionAccessor<object>
    {
        public List<int> SelectedInstanceIds
        {
            get { return AccessField<List<int>>("m_SelectedInstanceIDs", BindingFlags.Instance | BindingFlags.Public); }
        }

        public List<int> ExpandedInstanceIds
        {
            get { return AccessField<List<int>>("m_ExpandedInstanceIDs", BindingFlags.Instance | BindingFlags.Public); }
        }

        public ObjectListAreaState(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ObjectListAreaState")) { }

        public bool IsRenaming(int instanceId)
        {
            var renameOverlay = new RenameOverlay(AccessField<object>("m_RenameOverlay", BindingFlags.Instance | BindingFlags.Public));
            return renameOverlay.IsRenaming() && renameOverlay.UserData == instanceId;
        }
    }
}