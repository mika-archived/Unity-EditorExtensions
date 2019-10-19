using System.Linq;
using System.Text.RegularExpressions;

using MUE2.Editor.Controls;

using UnityEditor;

using UnityEngine;

namespace MUE2.Editor
{
    public class RenameAnimationClipProps : EditorWindow
    {
        private AnimationClip _animationClip;
        private string _query;
        private string _replaceTo;

        // ReSharper disable once UnusedMember.Local
        [MenuItem("Window/MUE2/Rename AnimationClip Properties")]
        private static void Init()
        {
            var window = GetWindow<RenameAnimationClipProps>();
            window.titleContent = new GUIContent("Rename AnimClip");

            window.Show();
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            // AnimationClip selector
            _animationClip = EditorContent.ObjectPicker<AnimationClip>("Animation Clip", _animationClip, false);

            // Query for replacing property
            _query = EditorContent.TextField("Property Query (Regex)", _query);

            // Strings for replacing to...
            _replaceTo = EditorContent.TextField("Replace To", _replaceTo);

            using (EditorContent.DisabledGroup(_animationClip == null || string.IsNullOrEmpty(_query) || string.IsNullOrEmpty(_replaceTo)))
                if (EditorContent.Button("Rename")) Rename();
        }

        private void Rename()
        {
            var compiledQuery = new Regex(_query);
            foreach (var binding in AnimationUtility.GetCurveBindings(_animationClip).Where(w => compiledQuery.IsMatch(w.propertyName)))
            {
                var curve = AnimationUtility.GetEditorCurve(_animationClip, binding);
                var replaced = binding;
                replaced.propertyName = compiledQuery.Replace(binding.propertyName, _replaceTo);
                Debug.Log(string.Format("Replaced {0} to {1}", binding.propertyName, replaced.propertyName));

                AnimationUtility.SetEditorCurve(_animationClip, binding, null);
                AnimationUtility.SetEditorCurve(_animationClip, replaced, curve);
            }
        }
    }
}