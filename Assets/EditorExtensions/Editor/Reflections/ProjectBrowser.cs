using System.Reflection;

using UnityEditor;

namespace Assets.EditorExtensions.Editor.Reflections
{
    public class ProjectBrowser : ReflectionAccessor<EditorWindow>
    {
        public TreeViewController AssetTree
        {
            get
            {
                var controller = AccessField<object>("m_AssetTree", BindingFlags.Instance | BindingFlags.NonPublic);
                return new TreeViewController(controller);
            }
        }

        public TreeViewState AssetTreeState
        {
            get
            {
                var state = AccessField<UnityEditor.IMGUI.Controls.TreeViewState>("m_AssetTreeState", BindingFlags.Instance | BindingFlags.NonPublic);
                return new TreeViewState(state);
            }
        }

        public ObjectListArea ListArea
        {
            get
            {
                var area = AccessField<object>("m_ListArea", BindingFlags.Instance | BindingFlags.NonPublic);
                return new ObjectListArea(area);
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

        private ProjectBrowser(EditorWindow instance) : base(instance, typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ProjectBrowser")) { }

        public static ProjectBrowser Create()
        {
            var browser = typeof(UnityEditor.Editor).Assembly.GetType("UnityEditor.ProjectBrowser");
            return new ProjectBrowser(EditorWindow.GetWindow(browser, false, null, false));
        }

        public bool IsTwoColumns()
        {
            return CallMethodAs<bool>("IsTwoColumns", BindingFlags.Instance | BindingFlags.NonPublic);
        }
    }
}