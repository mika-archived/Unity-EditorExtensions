using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class TreeViewController : ReflectionAccessor<object>
    {
        public TreeViewController(object instance) : base(instance, typeof(Editor).Assembly.GetType("UnityEditor.IMGUI.Controls.TreeViewController")) { }

        public Rect GetRectForRows(int startRow, int endRow, float rowWidth)
        {
            return CallMethodAs<Rect>("GetRectForRows", BindingFlags.Instance | BindingFlags.NonPublic, startRow, endRow, rowWidth);
        }

        public int GetRowIndex(int id)
        {
            var data = new TreeViewDataSource(AccessProperty<object>("data", BindingFlags.Instance | BindingFlags.Public));
            return data.GetRow(id);
        }
    }
}