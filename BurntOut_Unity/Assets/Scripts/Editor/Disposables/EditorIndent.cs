using System;
using UnityEditor;

/// <summary>
/// Class to keep contained elements indented using EditorGUI.indentLevel
/// </summary>
public class EditorIndent : IDisposable
{
    public EditorIndent()
    {
        EditorGUI.indentLevel++;
    }

    public void Dispose()
    {
        EditorGUI.indentLevel--;
    }
}
