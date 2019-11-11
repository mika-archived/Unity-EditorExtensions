using System.Reflection;

using MUE2.Editor.Reflection.Expressions;

using UnityEngine;

namespace MUE2.Editor.Reflection
{
    public class ObjectListLocalGroup : ReflectionClass
    {
        public ObjectListLocalGroup(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ObjectListArea+LocalGroup")) { }

        public int IndexOf(int instanceId)
        {
            return InvokeMethod<int>("IndexOf", BindingFlags.Instance | BindingFlags.Public, instanceId);
        }

        public Rect CalcRect(int index)
        {
            var grid = new VerticalGrid(InvokeMember<object>("m_Grid"));
            return grid.CalcRect(index, 0);
        }
    }
}