using System;
using UnityEditor;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout vertical group.
    /// Most uses of this class are to force GUI elements to use the correct indentation.
    /// </summary>
    public class Vertical : IDisposable
    {
        private int oldIndentation;

        public Vertical()
        {
            EditorGUILayout.BeginVertical();

            oldIndentation = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;
        }

        public void Dispose()
        {
            EditorGUI.indentLevel = oldIndentation;

            EditorGUILayout.EndVertical();
        }
    }
}
