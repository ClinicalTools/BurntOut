using OOEditor.Internal;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group styled as a toolbar.
    /// </summary>
    public static class Toolbar
    {
        private class DisposableToolbar : IDisposable
        {
            private readonly bool wasInHorizontal, wasInToolbar;
            public DisposableToolbar()
            {
                wasInHorizontal = OOEditorManager.InHorizontal;
                if (!wasInHorizontal)
                {
                    OOEditorManager.ResetHorizontalRect();
                    OOEditorManager.InHorizontal = true;
                }
                wasInToolbar = OOEditorManager.InToolbar;
                OOEditorManager.InToolbar = true;

                EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            }

            public void Dispose()
            {
                if (this != disposables.Pop())
                    Debug.LogError("Toolbar incorrectly disposed.");

                EditorGUILayout.EndHorizontal();

                OOEditorManager.InToolbar = wasInToolbar;
                OOEditorManager.InHorizontal = wasInHorizontal;
            }
        }
        
        private static readonly Stack<IDisposable> disposables = new Stack<IDisposable>();
        public static IDisposable Draw()
        {
            var disposable = new DisposableToolbar();
            disposables.Push(disposable);
            return disposable;
        }

        public static void EndDraw()
        {
            if (disposables.Count == 0)
                Debug.LogError("Toolbar incorrectly disposed.");
            else
                disposables.Peek().Dispose();
        }
    }
}
