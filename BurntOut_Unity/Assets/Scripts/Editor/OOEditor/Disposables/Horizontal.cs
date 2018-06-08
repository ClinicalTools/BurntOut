using OOEditor.Internal;
using System;
using UnityEditor;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group.
    /// </summary>
    public class Horizontal : IDisposable
    {
        public Horizontal()
        {
            if (OOEditorManager.InHorizontal++ == 0)
                OOEditorManager.ResetHorizontalRect();

            EditorGUILayout.BeginHorizontal();
        }

        public void Dispose()
        {
            EditorGUILayout.EndHorizontal();

            OOEditorManager.InHorizontal--;
        }
    }
}
