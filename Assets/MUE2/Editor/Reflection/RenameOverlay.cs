using System.Reflection;

using MUE2.Editor.Reflection.Expressions;

namespace MUE2.Editor.Reflection
{
    public class RenameOverlay : ReflectionClass
    {
        public int UserData => InvokeMember<int>("userData");

        public RenameOverlay(object instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.RenameOverlay")) { }

        public bool IsRenaming()
        {
            return InvokeMethod<bool>("IsRenaming", BindingFlags.Instance | BindingFlags.Public);
        }
    }
}