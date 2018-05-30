using System;
using UnityEditor;

namespace CtiEditor.Disposable
{
    /// <summary>
    /// Class to keep contained elements indented using EditorGUI.indentLevel
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