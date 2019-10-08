using System.Reflection;

using UnityEditor;

namespace Assets.Mikazuki.EditorExtensions.Scripts.Reflections
{
    public class ProjectBrowser : ReflectionAccessor<EditorWindow>
    {
        public TreeViewState AssetTreeState
        {
            get
            {
                var state = AccessField<UnityEditor.IMGUI.Controls.TreeViewState>("m_AssetTreeState", BindingFlags.Instance | BindingFlags.NonPublic);
                return new TreeViewState(state);
            }
        }

        public ObjectListAreaState ListAreaState
        {
            get
            {
                var state = AccessField<object>("m_ListAreaState", BindingFlags.Instance | BindingFlags.NonPublic);
                return new ObjectListAreaState(state);
            }
        }

        private ProjectBrowser(EditorWindow instance) : base(instance, typeof(Editor).Assembly.GetType("UnityEditor.ProjectBrowser")) { }

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