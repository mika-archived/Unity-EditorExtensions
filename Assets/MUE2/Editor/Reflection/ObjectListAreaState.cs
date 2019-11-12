using System.Collections.Generic;

using MUE2.Editor.Reflection.Expressions;

namespace MUE2.Editor.Reflection
{
    public class ObjectListAreaState : ReflectionClass
    {
        public List<int> SelectedInstanceIds => InvokeMember<List<int>>("m_SelectedInstanceIDs");

        public List<int> ExpandedInstanceIds => InvokeMember<List<int>>("m_ExpandedInstanceIDs");

        public ObjectListAreaState(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ObjectListAreaState")) { }

        public bool IsRenaming(int instanceId)
        {
            var renameOverlay = new RenameOverlay(InvokeMember<object>("m_RenameOverlay"));
            return renameOverlay.IsRenaming() && renameOverlay.UserData == instanceId;
        }
    }
}