using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// Manages the editing of a choice for a given scenario.
/// </summary>
public class ChoiceEditor
{
    private bool eventsFoldout;
    private bool optionsFoldout;
    private readonly List<bool> optionFoldout = new List<bool>();
    private readonly List<OptionEditor> optionEditors = new List<OptionEditor>();
    private readonly TasksEditor tasksEditor;

    private readonly Scenario scenario;
    private readonly Choice choice;
    private readonly ReorderableList list;
    

    public ChoiceEditor(Choice choice, Scenario scenario)
    {
        this.choice = choice;
        this.scenario = scenario;

        foreach (Option option in choice.Options)
        {
            optionFoldout.Add(false);
            optionEditors.Add(new OptionEditor(option, scenario));
        }

        tasksEditor = new TasksEditor(choice.Events, scenario);
    }

    public void Edit()
    {
        choice.Name = EditorGUILayout.TextField(new GUIContent("Name: ", "Name to be displayed in the editor"), choice.Name);

        choice.Text = EditorGUILayout.TextField(new GUIContent("Text: ", "Text to be displayed in game"), choice.Text);

        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        eventsFoldout = EditorGUILayout.Foldout(eventsFoldout, "Events", EditorStyles.foldout);
        EditorStyles.foldout.fontStyle = FontStyle.Normal;

        if (eventsFoldout)
            tasksEditor.Edit();


        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        optionsFoldout = EditorGUILayout.Foldout(optionsFoldout, "Options", EditorStyles.foldout);
        EditorStyles.foldout.fontStyle = FontStyle.Normal;
        if (optionsFoldout)
        {
            using (new EditorIndent())
            {
                EditorHelper.FoldoutListEdit(
                    // Add element
                    () =>
                    {
                        Option option = new Option();
                        choice.Options.Add(option);
                        optionEditors.Add(new OptionEditor(option, scenario));
                    },
                    // Move element
                    (int orig, int newPos) =>
                    {
                        Option option = choice.Options[orig];
                        choice.Options.RemoveAt(orig);
                        choice.Options.Insert(newPos, option);

                        OptionEditor optionEditor = optionEditors[orig];
                        optionEditors.RemoveAt(orig);
                        optionEditors.Insert(newPos, optionEditor);
                    },
                    // Remove element
                    (int i) =>
                    {
                        choice.Options.RemoveAt(i);
                        optionEditors.RemoveAt(i);

                    },
                    // Folded out display
                    (int i) =>
                    {
                        using (new EditorVertical(EditorStyles.helpBox))
                            optionEditors[i].Edit();
                    },
                    optionFoldout,
                    // Foldout title
                    (int i) => { return ("Option " + (i + 1) + " - " + choice.Options[i].Name); },
                    "Remove Option",
                    "Are you sure you want to delete this option?"
                );
            }
        }
    }
}
