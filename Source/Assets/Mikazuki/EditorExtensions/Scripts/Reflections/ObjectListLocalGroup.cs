using System.Reflection;

using UnityEditor;

using UnityEngine;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class ObjectListLocalGroup : ReflectionAccessor<object>
    {
        public ObjectListLocalGroup(object instance) : base(instance, typeof(Editor).Assembly.GetType("UnityEditor.ObjectListArea+LocalGroup")) { }

        public int IndexOf(int instanceId)
        {
            return CallMethodAs<int>("IndexOf", BindingFlags.Instance | BindingFlags.Public, instanceId);
        }

        public Rect CalcRect(int index)
        {
            var grid = new VerticalGrid(AccessField<object>("m_Grid", BindingFlags.Instance | BindingFlags.Public));
            return grid.CalcRect(index, 0);
        }
    }
}