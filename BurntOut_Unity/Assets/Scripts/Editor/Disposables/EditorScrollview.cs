using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class to keep contained elements within an EditorGUILayout ScrollView.
/// </summary>
public class EditorScrollView : IDisposable
{
    private Vector2 vec = new Vector2();

    public EditorScrollView()
    {
        EditorGUILayout.BeginScrollView(vec);
    }

    public void Dispose()
    {
        EditorGUILayout.EndScrollView();
    }
}
