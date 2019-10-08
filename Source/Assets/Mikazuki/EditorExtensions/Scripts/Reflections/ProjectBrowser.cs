using System.Reflection;

using UnityEditor;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class ProjectBrowser : ReflectionTypeBase
    {
        private ProjectBrowser(object instance) : base(instance) { }

        public static ProjectBrowser Create()
        {
            var browser = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
            return new ProjectBrowser(EditorWindow.GetWindow(browser, false, null, false));
        }

        public bool IsTwoColumns()
        {
            return CallMethodAsBool("IsTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}