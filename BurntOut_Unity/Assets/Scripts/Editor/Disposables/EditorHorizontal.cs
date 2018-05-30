using System;
using UnityEditor;
using UnityEngine;

namespace CtiEditor
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group.
    /// </summary>
    internal class EditorHorizontal : IDisposable
    {
        internal EditorHorizontal(GUIStyle style = null)
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