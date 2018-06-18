using OOEditor.Internal;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group.
    /// </summary>
    public static class Horizontal
    {
        private class DisposableHorizontal : IDisposable
        {
            private readonly bool wasInHorizontal;
            public DisposableHorizontal()
            {
                wasInHorizontal = OOEditorManager.InHorizontal;
                if (!wasInHorizontal)
                {
                    OOEditorManager.InHorizontal = true;
                    OOEditorManager.ResetHorizontalRect();
                }

                EditorGUILayout.BeginHorizontal();
            }

            public void Dispose()
            {
                if (this != disposables.Pop())
                    Debug.LogError("Horizontal incorrectly disposed.");

                EditorGUILayout.EndHorizontal();

                OOEditorManager.InHorizontal = wasInHorizontal;
            }
        }

        private static readonly Stack<IDisposable> disposables = new Stack<IDisposable>();
        public static IDisposable Draw()
        {
            var disposable = new DisposableHorizontal();
            disposables.Push(disposable);
            return disposable;
        }

        public static void EndDraw()
        {
            if (disposables.Count == 0)
                Debug.LogError("Horizontal incorrectly disposed.");
            else
                disposables.Peek().Dispose();
        }
    }
}
