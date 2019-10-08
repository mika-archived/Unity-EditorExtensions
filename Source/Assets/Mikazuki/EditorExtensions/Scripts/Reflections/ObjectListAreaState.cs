using System.Collections.Generic;
using System.Reflection;

using UnityEditor;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class ObjectListAreaState : ReflectionAccessor<object>
    {
        public List<int> SelectedInstanceIds
        {
            get { return AccessField<List<int>>("m_SelectedInstanceIDs", BindingFlags.Instance | BindingFlags.Public); }
        }

        public ObjectListAreaState(object instance) : base(instance, typeof(Editor).Assembly.GetType("UnityEditor.ObjectListAreaState")) { }

        public bool IsRenaming(int instanceId)
        {
            var renameOverlay = new RenameOverlay(AccessField<object>("m_RenameOverlay", BindingFlags.Instance | BindingFlags.Public));
            return renameOverlay.IsRenaming() && renameOverlay.UserData == instanceId;
        }
    }
}