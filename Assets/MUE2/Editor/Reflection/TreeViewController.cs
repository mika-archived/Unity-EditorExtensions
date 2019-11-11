using System.Reflection;

using MUE2.Editor.Reflection.Expressions;

using UnityEngine;

namespace MUE2.Editor.Reflection
{
    public class TreeViewController : ReflectionClass
    {
        public TreeViewController(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewController")) { }

        public Rect GetRectForRows(int startRow, int endRow, float rowWidth)
        {
            return InvokeMethod<Rect>("GetRectForRows", BindingFlags.Instance | BindingFlags.NonPublic, startRow, endRow, rowWidth);
        }

        public int GetRowIndex(int id)
        {
            var data = new TreeViewDataSource(InvokeMember<object>("data"));
            return data.GetRow(id);
        }
    }
}