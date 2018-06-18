using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within a ScrollView.
    /// </summary>
    public class ScrollView
    {
        private Vector2 scrollPos;

        private class DisposableScrollView : IDisposable
        {
            private bool disposed;
            private readonly Stack<IDisposable> disposables;

            public DisposableScrollView(ref Vector2 scrollPos)
            {
                scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
            }

            public void Dispose()
            {
                if (disposed)
                    Debug.LogError("ScrollView incorrectly disposed.");
                else
                    EditorGUILayout.EndScrollView();

                disposed = true;
            }
        }
        
        private IDisposable disposable;
        public IDisposable Draw()
        {
            disposable = new DisposableScrollView(ref scrollPos);
            return disposable;
        }
        public void EndDraw()
        {
            if (disposable == null)
                Debug.LogError("ScrollView incorrectly disposed.");
            else
                disposable.Dispose();
        }
    }
}