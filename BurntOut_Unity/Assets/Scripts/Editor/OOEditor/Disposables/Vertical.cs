using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout vertical group.
    /// Most uses of this class are to force GUI elements to use the correct indentation.
    /// </summary>
    public class Vertical : IDisposable
    {
        private int oldIndentLevel;

        public Vertical()
        {
            oldIndentLevel = EditorGUI.indentLevel;
            EditorGUI.indentLevel = 0;

            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(oldIndentLevel * 20);
            EditorGUILayout.BeginVertical();
        }

        public void Dispose()
        {
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();

            EditorGUI.indentLevel = oldIndentLevel;

        }
    }
}
