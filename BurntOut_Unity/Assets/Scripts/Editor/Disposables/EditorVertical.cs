using System;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Class to keep contained elements within an EditorGUILayout vertical group.
/// </summary>
public class EditorVertical : IDisposable
{
    private readonly int oldIndentLevel;

    public EditorVertical(GUIStyle style = null)
    {
        oldIndentLevel = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        EditorGUILayout.BeginHorizontal();
        GUILayout.Space(oldIndentLevel * 20);
        if (style == null)
            EditorGUILayout.BeginVertical();
        else
            EditorGUILayout.BeginVertical(style);
    }

    public void Dispose()
    {
        EditorGUILayout.EndVertical();
        EditorGUILayout.EndHorizontal();

        EditorGUI.indentLevel = oldIndentLevel;
    }
}
