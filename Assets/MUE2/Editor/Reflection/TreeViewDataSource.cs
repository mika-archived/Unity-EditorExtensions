using System.Reflection;

using MUE2.Editor.Reflection.Expressions;

namespace MUE2.Editor.Reflection
{
    public class TreeViewDataSource : ReflectionClass
    {
        public TreeViewDataSource(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewDataSource")) { }

        public int GetRow(int instanceId)
        {
            return InvokeMethod<int>("GetRow", BindingFlags.Instance | BindingFlags.Public, instanceId);
        }
    }
}