using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class to keep contained elements within an EditorGUILayout ScrollView.
/// </summary>
public class EditorScrollView : IDisposable
{
    public EditorScrollView(ref Vector2 scrollPosition)
    {
        scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
    }

    public void Dispose()
    {
        EditorGUILayout.EndScrollView();
    }
}
