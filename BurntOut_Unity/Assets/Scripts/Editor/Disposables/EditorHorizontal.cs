using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class to keep contained elements within an EditorGUILayout horizontal group.
/// </summary>
public class EditorHorizontal : IDisposable
{
    public EditorHorizontal(GUIStyle style = null)
    {
        if (style == null)
            EditorGUILayout.BeginHorizontal();
        else
            EditorGUILayout.BeginHorizontal(style);
    }

    public void Dispose()
    {
        EditorGUILayout.EndHorizontal();
    }
}
