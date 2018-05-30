using System;
using UnityEditor;
using UnityEngine;

namespace CtiEditor.Disposable
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group.
    /// </summary>
    internal class Horizontal : IDisposable
    {
        internal Horizontal(GUIStyle style = null)
        {
            CtiEditorGUI.InHorizontal++;

            if (style == null)
                EditorGUILayout.BeginHorizontal();
            else
                EditorGUILayout.BeginHorizontal(style);
        }

        public void Dispose()
        {
            CtiEditorGUI.InHorizontal--;

            EditorGUILayout.EndHorizontal();
        }
    }
}