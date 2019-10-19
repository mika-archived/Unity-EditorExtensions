using System;

using UnityEditor;

namespace MUE2.Editor.Controls
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