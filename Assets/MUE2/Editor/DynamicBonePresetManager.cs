using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

using MUE2.Editor.Controls;
using MUE2.Editor.Models;
using MUE2.Editor.Reflections;

using UnityEditor;

using UnityEngine;

namespace MUE2.Editor
{
    /// <summary>
    ///     save and restore DynamicBone components by naming convention
    /// </summary>
    internal class DynamicBonePresetManager : EditorWindow
    {
        private const string DynamicBoneFullName = "DynamicBone";
        private const string DynamicBoneColliderFullName = "DynamicBoneCollider";
        private const string DynamicBonePlaneColliderFullName = "DynamicBonePlaneCollider";
        private readonly List<DynamicBoneConfiguration> _configurations;
        private readonly Dictionary<int, bool> _enabledComponents;
        private GameObject _gameObject;
        private string _name;
        private DynamicBonePreset _preset;
        private TabMode _tabMode;

        public DynamicBonePresetManager()
        {
            _enabledComponents = new Dictionary<int, bool>();
            _configurations = new List<DynamicBoneConfiguration>();
        }

        // ReSharper disable once UnusedMember.Local
        [MenuItem("Window/MUE2/DynamicBone Preset Manager")]
        public static void Init()
        {
            var window = GetWindow<DynamicBonePresetManager>();
            window.titleContent = new GUIContent("DynamicBone Preset Manager");

            window.Show();
        }

        // ReSharper disable once InconsistentNaming
        // ReSharper disable once UnusedMember.Global
        public void OnGUI()
        {
            EditorContent.Space();

            EditorContent.BeginObjectChanged();
            _tabMode = (TabMode) EditorContent.Tab((int) _tabMode, "Create Preset", "Apply Preset");
            if (EditorContent.IsObjectChanged())
            {
                _gameObject = null;
                _enabledComponents.Clear();
                _configurations.Clear();
                _preset = null;
            }

            EditorContent.Space();

            switch (_tabMode)
            {
                case TabMode.Save:
                    CreatePresetLayout();
                    break;

                case TabMode.Restore:
                    ApplyPresetLayout();
                    break;
            }
        }

        private void CreatePresetLayout()
        {
            EditorContent.Label("Create DynamicBone preset from existing GameObject tree.");
            EditorGUIUtility.labelWidth = 200;
            _gameObject = EditorContent.ObjectPicker<GameObject>("Root GameObject", _gameObject, true);
            if (ReferenceEquals(_gameObject, null))
                return;

            var components = new List<Component>();
            components.AddRange(_gameObject.GetComponentsInChildren(TypeFinder.GetType(DynamicBoneFullName)));
            components.AddRange(_gameObject.GetComponentsInChildren(TypeFinder.GetType(DynamicBoneColliderFullName)));
            components.AddRange(_gameObject.GetComponentsInChildren(TypeFinder.GetType(DynamicBonePlaneColliderFullName)));

            if (components.Count == 0)
                return;

            // differ
            {
                var added = components.Where(w => !_enabledComponents.ContainsKey(w.GetInstanceID())).ToList();
                var removed = _enabledComponents.Where(w => ReferenceEquals(components.SingleOrDefault(v => v.GetInstanceID() == w.Key), null)).ToList();

                foreach (var component in added)
                    _enabledComponents.Add(component.GetInstanceID(), false);
                foreach (var instanceId in removed.Select(w => w.Key))
                    _enabledComponents.Remove(instanceId);
            }

            EditorContent.Space();
            EditorContent.Label("Enabled Components:");
            foreach (var component in components)
            {
                var instanceId = component.GetInstanceID();
                _enabledComponents[instanceId] = EditorContent.Checkbox(component.ToString(), _enabledComponents[instanceId]);
            }

            if (_enabledComponents.Any(w => w.Value))
            {
                EditorContent.Space();
                EditorContent.Label("Naming Conventions:");
            }

            foreach (var component in _enabledComponents.Where(w => w.Value).Select(w => components.Single(v => v.GetInstanceID() == w.Key)))
            {
                var convention = PropertyTransformer.TransformToStringPath(component);
                var instance = _configurations.SingleOrDefault(w => w.OriginalInstanceId == component.GetInstanceID());
                if (ReferenceEquals(instance, null))
                {
                    instance = new DynamicBoneConfiguration();
                    instance.Name = instance.NamingConvention = convention;
                    instance.Component = component;
                    instance.OriginalInstanceId = component.GetInstanceID();
                    _configurations.Add(instance);
                }

                instance.NamingConvention = EditorContent.TextField(component.ToString(), instance.NamingConvention);
            }

            // cleanup instances
            foreach (var configuration in _enabledComponents.Where(w => !w.Value).Select(w => _configurations.SingleOrDefault(v => w.Key == v.OriginalInstanceId)).Where(w => !ReferenceEquals(w, null)).ToList())
                _configurations.Remove(configuration);

            EditorContent.Space();
            EditorContent.Label("DynamicBone Preset Manager convert hierarchy tree to the below name:");
            EditorContent.Label("Avatar → Armature → Hips → ... to Avatar/Armature/Hips/...");
            EditorContent.Label("If you want to preserve the tree structure, use `/` and `[^/]*?` in the naming convention.");

            using (EditorContent.DisabledGroup(_configurations.Count == 0))
                _name = EditorContent.TextField("Preset Name", _name);

            using (EditorContent.DisabledGroup(string.IsNullOrEmpty(_name) || _configurations.Count == 0))
                if (EditorContent.Button("Create DynamicBone Preset")) CreatePreset();
        }

        private void ApplyPresetLayout()
        {
            EditorContent.Label("Apply DynamicBone preset to existing GameObject tree.");
            _gameObject = EditorContent.ObjectPicker<GameObject>("Root GameObject", _gameObject, true);
            _preset = EditorContent.ObjectPicker<DynamicBonePreset>("Preset", _preset, false);

            using (EditorContent.DisabledGroup(_gameObject == null || _preset == null))
                if (EditorContent.Button("Apply DynamicBone Preset")) ApplyPreset();
        }

        private void CreatePreset()
        {
            var preset = new DynamicBonePreset();
            foreach (var configuration in _configurations)
            {
                configuration.StoreConfiguration();
                preset.Configs.Add(configuration);
            }

            if (!Directory.Exists("Assets/DynamicBonePresets"))
                AssetDatabase.CreateFolder("Assets", "DynamicBonePresets");
            AssetDatabase.CreateAsset(preset, string.Format("Assets/DynamicBonePresets/{0}.asset", _name));
        }

        private void ApplyPreset()
        {
            // 1st, apply DynamicBoneCollider
            foreach (var collider in _preset.Configs.Where(w => w.EffectiveAs == "DynamicBoneCollider"))
                foreach (var gameObject in FindGameObjectByPath(collider.NamingConvention))
                {
                    if (gameObject.GetComponent(TypeFinder.GetType(DynamicBoneColliderFullName)) != null)
                        continue; // already attached

                    var component = gameObject.AddComponent(TypeFinder.GetType(DynamicBoneColliderFullName));
                    collider.DynamicBoneColliderProperties.ApplyProperties(component);
                }

            // 2nd, apply DynamicBone
            foreach (var bone in _preset.Configs.Where(w => w.EffectiveAs == "DynamicBone"))
                foreach (var gameObject in FindGameObjectByPath(bone.NamingConvention))
                {
                    if (gameObject.GetComponent(TypeFinder.GetType(DynamicBoneFullName)) != null)
                        continue; // already attached

                    var component = gameObject.AddComponent(TypeFinder.GetType(DynamicBoneFullName));
                    bone.DynamicBoneProperties.ApplyProperties(component, _gameObject);
                }
        }

        private List<GameObject> FindGameObjectByPath(string path)
        {
            var compiled = new Regex(string.Format("^{0}$", path));
            var objects = _gameObject.GetComponentsInChildren<Transform>();
            return objects.Select(w => w.gameObject).Where(w => compiled.IsMatch(PropertyTransformer.TransformToStringPath(w))).ToList();
        }

        private enum TabMode
        {
            Save,

            Restore
        }
    }
}