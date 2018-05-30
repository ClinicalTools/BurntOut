using CtiEditor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Provides basic editor methods to increase ease of displaying things.
/// For now it just has custom list methods.
/// </summary>
public static class EditorHelper
{
    public static readonly Color ErrorColor = new Color(1f, .4f, .4f);
    public static readonly Color ContinueColor = new Color(.7f, 1f, .7f);
    public static readonly Color TryAgainColor = new Color(1f, 1f, .7f);
    public static readonly Color EndColor = new Color(1f, .7f, .7f);

    // Displays a list in the editor
    public static void ListEdit(int count, Action addElem, Action<int, int> moveElem, Action<int> removeElem,
        Action<int> display, string removeTitle, string removeDesc)
    {
        ListEdit(count, addElem, moveElem, removeElem, display, null, null, null, removeTitle, removeDesc);
    }

    // Displays a list in the editor with foldouts
    public static void FoldoutListEdit(Action addElem, Action<int, int> moveElem, Action<int> removeElem,
        Action<int> display, List<bool> foldouts, Func<int, string> name, Func<int, Color> color, string removeTitle, string removeDesc)
    {
        ListEdit(foldouts.Count, addElem, moveElem, removeElem, display, foldouts, name, color, removeTitle, removeDesc);
    }

    private static void ListEdit(int count, Action addElem, Action<int, int> moveElem, Action<int> removeElem,
        Action<int> display, List<bool> foldouts, Func<int, string> name, Func<int, Color> color, string removeTitle, string removeDesc)
    {
        for (int i = 0; i < count; i++)
        {
            using (CtiEditorGUI.Horizontal())
            {
                if (foldouts != null)
                {
                    var lastColor = GUI.contentColor;

                    GUI.contentColor = color(i);
                    foldouts[i] = CtiEditorGUI.Foldout(foldouts[i], name(i));

                    GUI.contentColor = lastColor;
                }
                else
                {
                    display(i);
                }

                // Button to move the element up (if it's not at the top)
                if (i > 0)
                {
                    if (CtiEditorGUI.Button("▲", null, null, CtiEditorGUI.Width(40)))
                    {
                        moveElem(i, i - 1);

                        if (foldouts != null)
                        {
                            bool folded = foldouts[i];
                            foldouts.RemoveAt(i);
                            foldouts.Insert(i - 1, folded);
                        }
                    }
                }
                else
                {
                    CtiEditorGUI.LabelField(" ", null, null, CtiEditorGUI.Width(40));
                }

                // Button to move the element down (if it's not at the bottom)
                if (i < count - 1)
                {
                    if (CtiEditorGUI.Button("▼", null, null, CtiEditorGUI.Width(40)))
                    {
                        moveElem(i, i + 1);

                        if (foldouts != null)
                        {
                            bool folded = foldouts[i];
                            foldouts.RemoveAt(i);
                            foldouts.Insert(i + 1, folded);
                        }
                    }
                }
                else
                {
                    CtiEditorGUI.LabelField(" ", null, null, CtiEditorGUI.Width(40));
                }

                if (CtiEditorGUI.Button("X", null, null, CtiEditorGUI.Width(40)))
                {
                    if (EditorUtility.DisplayDialog(removeTitle, removeDesc, "Delete", "Cancel"))
                    {
                        removeElem(i);
                        if (foldouts != null)
                            foldouts.RemoveAt(i);
                        i--;
                        count--;

                        continue;
                    }
                }
            }

            if (foldouts != null && foldouts[i])
                using (new EditorIndent())
                    display(i);
        }

        // Add new element button
        using (CtiEditorGUI.Horizontal())
        {
            GUILayout.Space(EditorGUI.indentLevel * 20);


            if (CtiEditorGUI.Button("+", null, null, CtiEditorGUI.Width(128)))
            {
                addElem();
                if (foldouts != null)
                    foldouts.Add(false);

                count++;
            }
        }
    }
}
