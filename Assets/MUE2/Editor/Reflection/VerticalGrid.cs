using System.Reflection;

using MUE2.Editor.Reflection.Expressions;

using UnityEngine;

namespace MUE2.Editor.Reflection
{
    public class VerticalGrid : ReflectionClass
    {
        public VerticalGrid(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.VerticalGrid")) { }

        public Rect CalcRect(int itemIdx, float yOffset)
        {
            return InvokeMethod<Rect>("CalcRect", BindingFlags.Instance | BindingFlags.Public, itemIdx, yOffset);
        }
    }
}