using System;

using UnityEditor;

namespace MUE2.Editor.Controls
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