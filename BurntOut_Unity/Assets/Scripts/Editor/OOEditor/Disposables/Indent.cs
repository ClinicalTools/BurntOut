using System;
using UnityEditor;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements indented.
    /// </summary>
    public class Indent : IDisposable
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
