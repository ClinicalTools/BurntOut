using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Class to keep contained elements within a ScrollView.
    /// </summary>
    public class ScrollView : IDisposable
    {
        public ScrollView(ref Vector2 scrollPosition)
        {
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
        }

        public void Dispose()
        {
            EditorGUILayout.EndScrollView();
        }
    }
}