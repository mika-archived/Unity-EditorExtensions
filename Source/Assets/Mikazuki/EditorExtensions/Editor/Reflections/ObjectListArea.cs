using System.Reflection;

namespace Assets.Mikazuki.EditorExtensions.Editor.Reflections
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

        public ObjectListArea(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ObjectListArea")) { }
    }
}