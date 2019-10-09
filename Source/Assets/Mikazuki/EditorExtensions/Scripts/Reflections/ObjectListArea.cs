using System.Reflection;

using UnityEditor;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class ObjectListArea : ReflectionAccessor<object>
    {
        public ObjectListLocalGroup LocalAssets
        {
            get
            {
                var localAssets = AccessField<object>("m_LocalAssets", BindingFlags.Instance | BindingFlags.NonPublic);
                return new ObjectListLocalGroup(localAssets);
            }
        }

        public ObjectListArea(object instance) : base(instance, typeof(Editor).Assembly.GetType("UnityEditor.ObjectListArea")) { }
    }
}