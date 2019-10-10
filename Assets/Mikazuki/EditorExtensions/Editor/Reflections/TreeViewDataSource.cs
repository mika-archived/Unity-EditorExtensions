using System.Reflection;

namespace Assets.Mikazuki.EditorExtensions.Editor.Reflections
{
    public class TreeViewDataSource : ReflectionAccessor<object>
    {
        public TreeViewDataSource(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewDataSource")) { }

        public int GetRow(int instanceId)
        {
            return CallMethodAs<int>("GetRow", BindingFlags.Instance | BindingFlags.Public, instanceId);
        }
    }
}