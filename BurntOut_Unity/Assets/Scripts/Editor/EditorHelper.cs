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
    // Displays a list in the editor
    public static void ListEdit(int count, Action addElem, Action<int, int> moveElem, Action<int> removeElem,
        Action<int> display, string removeTitle, string removeDesc)
    {
        ListEdit(count, addElem, moveElem, removeElem, display, null, null, removeTitle, removeDesc);
    }

    // Displays a list in the editor with foldouts
    public static void FoldoutListEdit(Action addElem, Action<int, int> moveElem, Action<int> removeElem,
        Action<int> display, List<bool> foldouts, Func<int, string> name, string removeTitle, string removeDesc)
    {
        ListEdit(foldouts.Count, addElem, moveElem, removeElem, display, foldouts, name, removeTitle, removeDesc);
    }

    private static void ListEdit(int count, Action addElem, Action<int, int> moveElem, Action<int> removeElem,
        Action<int> display, List<bool> foldouts, Func<int, string> name, string removeTitle, string removeDesc)
    {
        for (int i = 0; i < count; i++)
        {
            using (new EditorHorizontal())
            {
                if (foldouts != null)
                    foldouts[i] = EditorGUILayout.Foldout(foldouts[i], name(i));
                else
                    display(i);

                // Button to move the element up (if it's not at the top)
                if (i > 0)
                {
                    if (GUILayout.Button("▲", GUILayout.Width(40)))
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
                    EditorGUILayout.LabelField(" ", GUILayout.Width(40));
                }

                // Button to move the element down (if it's not at the bottom)
                if (i < count - 1)
                {
                    if (GUILayout.Button("▼", GUILayout.Width(40)))
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
                    EditorGUILayout.LabelField(" ", GUILayout.Width(40));
                }

                if (GUILayout.Button("X", GUILayout.Width(40)))
                {
                    if (EditorUtility.DisplayDialog(removeTitle, removeDesc, "Delete", "Cancel")) { 
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
        using (new EditorHorizontal())
        {
            GUILayout.Space(EditorGUI.indentLevel * 20);


            if (GUILayout.Button("+", GUILayout.Width(128)))
            {
                addElem();
                if (foldouts != null)
                    foldouts.Add(false);

                count++;
            }
        }
    }
}
