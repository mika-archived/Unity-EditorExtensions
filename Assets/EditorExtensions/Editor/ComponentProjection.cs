using System.Collections.Generic;
using System.Linq;

using Assets.EditorExtensions.Editor.Controls;

using UnityEditor;

using UnityEngine;

namespace Assets.EditorExtensions.Editor
{
    public class ComponentProjection : EditorWindow
    {
        private readonly Dictionary<int, bool> _projections;
        private Component[] _components;
        private GameObject _destinationGameObject;
        private GameObject _sourceGameObject;

        public ComponentProjection()
        {
            _projections = new Dictionary<int, bool>();
        }

        // ReSharper disable once UnusedMember.Local
        [MenuItem("Window/Editor Extensions/Component Projection")]
        public static void Init()
        {
            var window = GetWindow<ComponentProjection>();
            window.titleContent = new GUIContent("Component Projection");

            window.Show();
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            EditorContent.BeginObjectChanged();
            _sourceGameObject = EditorContent.ObjectPicker<GameObject>("Source GameObject", _sourceGameObject, true);

            if (EditorContent.EndObjectChanged())
                _projections.Clear();

            if (_sourceGameObject == null)
            {
                EditorContent.Label("Select the GameObject that you want to project the Component.");
                return;
            }

            _components = _sourceGameObject.GetComponents<Component>();

            EditorContent.Label("Components:");
            foreach (var component in _components)
            {
                var instanceId = component.GetInstanceID();
                if (!_projections.ContainsKey(instanceId))
                    _projections.Add(instanceId, false);
                _projections[instanceId] = EditorContent.Checkbox(ObjectNames.GetInspectorTitle(component), _projections[instanceId]);
            }

            EditorContent.Space();

            _destinationGameObject = EditorContent.ObjectPicker<GameObject>("Destination GameObject", _destinationGameObject, true);

            using (EditorContent.DisabledGroup(_destinationGameObject == null || _projections.All(w => !w.Value)))
            {
                if (EditorContent.Button("Projection"))
                    foreach (var sourceComponent in _components.Where(w => _projections[w.GetInstanceID()]))
                    {
                        // ReSharper disable once PossibleNullReferenceException
                        var destComponent = _destinationGameObject.AddComponent(sourceComponent.GetType());

                        using (var sourceProperties = new SerializedObject(sourceComponent))
                        using (var destProperties = new SerializedObject(destComponent))
                        {
                            var iterator = sourceProperties.GetIterator();
                            while (iterator.NextVisible(true))
                                destProperties.CopyFromSerializedProperty(iterator);
                            destProperties.ApplyModifiedProperties();
                        }
                    }
            }
        }
    }
}