using System;

using UnityEditor;

namespace Assets.EditorExtensions.Editor.Controls
{
    internal class DisabledGroup : IDisposable
    {
        public DisabledGroup(bool disabled)
        {
            EditorGUI.BeginDisabledGroup(disabled);
        }

        public void Dispose()
        {
            EditorGUI.EndDisabledGroup();
        }
    }
}