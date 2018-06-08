using OOEditor.Internal;
using System;
using UnityEditor;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group styled as a toolbar.
    /// </summary>
    public class Toolbar : IDisposable
    {
        public Toolbar()
        {
            if (OOEditorManager.InHorizontal++ == 0)
                OOEditorManager.ResetHorizontalRect();
            OOEditorManager.InToolbar++;

            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
        }

        public void Dispose()
        {
            EditorGUILayout.EndHorizontal();

            OOEditorManager.InToolbar--;
            OOEditorManager.InHorizontal--;
        }
    }
}
