using CtiEditor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Manages the editing of a scenario.
/// </summary>
public class ScenarioEditor
{
    private bool actorsFoldout;
    private bool choicesFoldout;
    private readonly List<bool> choiceFoldout = new List<bool>();
    private readonly List<ChoiceEditor> choiceEditors = new List<ChoiceEditor>();
    public readonly Scenario scenario;
    private float endNarrativeWidth;

    public ScenarioEditor(Scenario scenario)
    {
        this.scenario = scenario;
        foreach (Choice choice in scenario.Choices)
        {
            choiceFoldout.Add(false);
            choiceEditors.Add(new ChoiceEditor(choice, scenario));
        }
    }

    public void Edit()
    {
        scenario.name = CtiEditorGUI.TextField(scenario.name, "Name: ", "Name to be displayed in the editor");

        using (CtiEditorGUI.LabelFontStyle(FontStyle.Bold))
            actorsFoldout = CtiEditorGUI.Foldout(actorsFoldout, "Actors");

        if (actorsFoldout)
        {
            using (CtiEditorGUI.Indent())
            {
                EditorHelper.ListEdit(
                    scenario.Actors.Count,
                    // Add element
                    () =>
                    {
                        scenario.Actors.Add(
                            new Actor(scenario.Actors.ToArray()));
                    },
                    // Move element
                    (int orig, int newPos) =>
                    {
                        Actor actor = scenario.Actors[orig];
                        scenario.Actors.RemoveAt(orig);
                        scenario.Actors.Insert(newPos, actor);
                    },
                    // Remove element
                    (int i) => { scenario.Actors.RemoveAt(i); },
                    // Display element
                    (int i) =>
                    {
                        scenario.Actors[i].name = CtiEditorGUI.TextField(scenario.Actors[i].name);
                    },
                    "Remove Actor",
                    "Are you sure you want to delete this actor?"
                );
            }
        }

        using (CtiEditorGUI.LabelFontStyle(FontStyle.Bold))
            choicesFoldout = CtiEditorGUI.Foldout(choicesFoldout, "Choices");

        if (choicesFoldout)
        {
            using (CtiEditorGUI.Indent())
            {
                EditorHelper.FoldoutListEdit(
                    // Add element
                    () =>
                    {
                        Choice choice = new Choice();
                        scenario.Choices.Add(choice);
                        choiceEditors.Add(new ChoiceEditor(choice, scenario));
                    },
                    // Move element
                    (int orig, int newPos) =>
                    {
                        Choice choice = scenario.Choices[orig];
                        scenario.Choices.RemoveAt(orig);
                        scenario.Choices.Insert(newPos, choice);

                        ChoiceEditor choiceEditor = choiceEditors[orig];
                        choiceEditors.RemoveAt(orig);
                        choiceEditors.Insert(newPos, choiceEditor);
                    },
                    // Remove element
                    (int i) =>
                    {
                        scenario.Choices.RemoveAt(i);
                        choiceEditors.RemoveAt(i);

                    },
                    // Folded out display
                    (int i) =>
                    {
                        using (CtiEditorGUI.Container())
                            choiceEditors[i].Edit();
                    },
                    choiceFoldout,
                    // Foldout title
                    (int i) => { return ("Choice " + (i + 1) + " - " + scenario.Choices[i].name); },
                    // Foldout color
                    (int i) =>
                    {
                        // Red if there is no option to continue
                        foreach (Option option in scenario.Choices[i].Options)
                            if (option.result == OptionResults.CONTINUE)
                                return GUI.contentColor;

                        return EditorHelper.ErrorColor;
                    },
                    "Remove Choice",
                    "Are you sure you want to delete this choice?"
                );
            }
        }

        CtiEditorGUI.LabelField("End Narrative:");
        EditorStyles.textField.wordWrap = true;
        scenario.endNarrative = CtiEditorGUI.TextArea(scenario.endNarrative, ref endNarrativeWidth);
    }
}
