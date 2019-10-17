using System.Linq;
using System.Text.RegularExpressions;

using Assets.EditorExtensions.Editor.Controls;

using UnityEditor;

using UnityEngine;

namespace Assets.EditorExtensions.Editor
{
    public class AnimationClipBulkRename : EditorWindow
    {
        private readonly Regex _regexPattern = new Regex("^~.*~$", RegexOptions.Compiled);
        private AnimationClip _animationClip;
        private string _query;
        private string _replaceTo;

        // ReSharper disable once UnusedMember.Local
        [MenuItem("Window/MUE2/AnimationClip Bulk Rename")]
        private static void Init()
        {
            var window = GetWindow<AnimationClipBulkRename>();
            window.titleContent = new GUIContent("AnimClip BR");

            window.Show();
        }

        // ReSharper disable once UnusedMember.Local
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            // AnimationClip selector
            _animationClip = EditorContent.ObjectPicker<AnimationClip>("Animation Clip", _animationClip, false);

            // Query for replacing property
            _query = EditorContent.TextField("Property Query", _query);

            // Strings for replacing to...
            _replaceTo = EditorContent.TextField("Replace To", _replaceTo);

            using (EditorContent.DisabledGroup(_animationClip == null || string.IsNullOrEmpty(_query) || string.IsNullOrEmpty(_replaceTo)))
                if (EditorContent.Button("Bulk Rename")) BulkRename();

            EditorContent.Space();
            EditorContent.Label("How to use:");
            using (EditorContent.Indent())
            {
                EditorContent.Label("1. Select an animation clip from assets that you want to rename properties");
                EditorContent.Label("2. Enter the name of the property that you want to rename in \"Property Query\"");
                using (EditorContent.Indent())
                    EditorContent.Label("Regular Expressions are supported. Delimiter is \"~\"");
                EditorContent.Label("3. Enter the new name of the property in \"Replace To\"");
                EditorContent.Label("4. Click the \"Bulk Rename\" button to proceed");
            }
        }

        private void BulkRename()
        {
            var isRegexMode = _regexPattern.IsMatch(_query);
            var compiledQuery = isRegexMode ? new Regex(_query.Substring(1, _query.Length - 2)) : null;

            foreach (var binding in AnimationUtility.GetCurveBindings(_animationClip).ToList())
            {
                var curve = AnimationUtility.GetEditorCurve(_animationClip, binding);
                var replaced = isRegexMode ? RenameWithRegex(binding, compiledQuery) : RenameWithPlain(binding, _query);

                AnimationUtility.SetEditorCurve(_animationClip, binding, null);
                AnimationUtility.SetEditorCurve(_animationClip, replaced, curve);
            }
        }

        private EditorCurveBinding RenameWithRegex(EditorCurveBinding binding, Regex query)
        {
            if (query.IsMatch(binding.propertyName))
                binding.propertyName = query.Replace(binding.propertyName, _replaceTo);
            return binding;
        }

        private EditorCurveBinding RenameWithPlain(EditorCurveBinding binding, string query)
        {
            if (binding.propertyName == query)
                binding.propertyName = _replaceTo;
            return binding;
        }
    }
}