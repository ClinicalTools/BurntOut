using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout GUIContainer group.
    /// </summary>
    public static class GUIContainer
    {
        private class DisposableGUIContainer : IDisposable
        {
            public DisposableGUIContainer()
            {
                EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            }

            public void Dispose()
            {
                if (this != disposables.Pop())
                    Debug.LogError("GUIContainer incorrectly disposed.");

                EditorGUILayout.EndVertical();
            }
        }

        private static readonly Stack<IDisposable> disposables = new Stack<IDisposable>();
        public static IDisposable Draw()
        {
            var disposable = new DisposableGUIContainer();
            disposables.Push(disposable);
            return disposable;
        }

        public static void EndDraw()
        {
            if (disposables.Count == 0)
                Debug.LogError("GUIContainer incorrectly disposed.");
            else
                disposables.Peek().Dispose();
        }
    }
}
