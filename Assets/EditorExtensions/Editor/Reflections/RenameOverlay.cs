using System.Reflection;

namespace Assets.EditorExtensions.Editor.Reflections
{
    public class RenameOverlay : ReflectionAccessor<object>
    {
        public int UserData
        {
            get { return AccessPropertyAs<int>("userData", BindingFlags.Instance | BindingFlags.Public); }
        }

        public RenameOverlay(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.RenameOverlay")) { }

        public bool IsRenaming()
        {
            return CallMethodAs<bool>("IsRenaming", BindingFlags.Instance | BindingFlags.Public);
        }
    }
}