using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements indented.
    /// </summary>
    public static class Indent
    {
        // Default width seems to be 20 with EditorGUI.indentLevel
        private const int INDENT_WIDTH = 20;

        private class DisposableVertical : IDisposable
        {
            public DisposableVertical()
            {
                EditorGUILayout.BeginHorizontal();
                GUILayout.Space(INDENT_WIDTH);
                EditorGUILayout.BeginVertical();
            }

            public void Dispose()
            {
                if (this != disposables.Pop())
                    Debug.LogError("Indent incorrectly disposed.");

                EditorGUILayout.EndVertical();
                EditorGUILayout.EndHorizontal();
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
                Debug.LogError("Indent incorrectly disposed.");
            else
                disposables.Peek().Dispose();
        }
    }
}
