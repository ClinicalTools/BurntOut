using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout vertical group.
    /// Most uses of this class are to force GUI elements to use the correct indentation.
    /// </summary>
    public static class Vertical
    {
        private class DisposableVertical : IDisposable
        {
            public DisposableVertical()
            {
                EditorGUILayout.BeginVertical();
            }

            public void Dispose()
            {
                if (this != disposables.Pop())
                    Debug.LogError("Vertical incorrectly disposed.");

                EditorGUILayout.EndVertical();
            }
        }

        private static readonly Stack<IDisposable> disposables = new Stack<IDisposable>();
        public static IDisposable Draw()
        {
            var disposable = new DisposableVertical();
            disposables.Push(disposable);
            return disposable;
        }

        public static void EndDraw()
        {
            if (disposables.Count == 0)
                Debug.LogError("Vertical incorrectly disposed.");
            else
                disposables.Peek().Dispose();
        }
    }
}
