using System;

using UnityEditor;

namespace Assets.EditorExtensions.Editor.Controls
{
    internal class Indent : IDisposable
    {
        public Indent()
        {
            EditorGUI.indentLevel++;
        }

        public void Dispose()
        {
            EditorGUI.indentLevel--;
        }
    }
}