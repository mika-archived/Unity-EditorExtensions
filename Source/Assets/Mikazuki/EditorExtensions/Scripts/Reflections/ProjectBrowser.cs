using System.Reflection;

using UnityEditor;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class ProjectBrowser : ReflectionTypeBase<object>
    {
        private ProjectBrowser(object instance) : base(instance) { }
        private ProjectBrowser(object instance) : base(instance, typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser")) { }

        public static ProjectBrowser Create()
        {
            var browser = typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
            return new ProjectBrowser(EditorWindow.GetWindow(browser, false, null, false));
        }

        public bool IsTwoColumns()
        {
            return CallMethodAs<bool>("IsTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}