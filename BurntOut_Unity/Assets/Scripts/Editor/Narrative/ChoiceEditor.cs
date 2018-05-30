using CtiEditor;
using System.Collections.Generic;
using UnityEditor;
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
        choice.name = CtiEditorGUI.TextField(choice.name, "Name: ", "Name to be displayed in the editor");

        using (CtiEditorGUI.LabelFontStyle(FontStyle.Bold))
            eventsFoldout = CtiEditorGUI.Foldout(eventsFoldout, "Events");

        if (eventsFoldout)
            using (CtiEditorGUI.Indent())
                tasksEditor.Edit();

        choice.text = CtiEditorGUI.TextField(choice.text, "Text: ", "Text to be displayed in game");

        using (CtiEditorGUI.LabelFontStyle(FontStyle.Bold))
            optionsFoldout = CtiEditorGUI.Foldout(optionsFoldout, "Options");

        if (optionsFoldout)
        {
            using (CtiEditorGUI.Indent())
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
                        using (CtiEditorGUI.Container())
                            optionEditors[i].Edit();
                    },
                    optionFoldout,
                    // Foldout title
                    (int i) => { return ("Option " + (i + 1) + " - " + choice.Options[i].name); },
                    // Foldout color
                    (int i) =>
                    {
                        if (choice.Options[i].result == OptionResults.CONTINUE)
                            return EditorHelper.ContinueColor;
                        else if (choice.Options[i].result == OptionResults.TRY_AGAIN)
                            return EditorHelper.TryAgainColor;
                        else if (choice.Options[i].result == OptionResults.END)
                            return EditorHelper.EndColor;

                        return GUI.contentColor;
                    },
                    "Remove Option",
                    "Are you sure you want to delete this option?"
                );
            }
        }
    }
}
