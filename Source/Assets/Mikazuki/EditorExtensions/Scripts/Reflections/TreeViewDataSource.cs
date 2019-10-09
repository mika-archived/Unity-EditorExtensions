using System.Reflection;

using UnityEditor;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class TreeViewDataSource : ReflectionAccessor<object>
    {
        public TreeViewDataSource(object instance) : base(instance, typeof(Editor).Assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewDataSource")) { }

        public int GetRow(int instanceId)
        {
            return CallMethodAs<int>("GetRow", BindingFlags.Instance | BindingFlags.Public, instanceId);
        }
    }
}