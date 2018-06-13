using CtiEditor;
using OOEditor;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Manages the editing of a scenario.
/// </summary>
public class ScenarioEditor
{
    //private bool actorsFoldout;
    //private bool choicesFoldout;
    private readonly List<bool> choiceFoldout = new List<bool>();
    private readonly List<ChoiceEditor> choiceEditors = new List<ChoiceEditor>();
    public readonly Scenario scenario;

    public static Scenario CurrentScenario { get; private set; }

    private TextField nameField;
    private Foldout actorsFoldout, choicesFoldout;
    private LabelField endNarrativeLabel;
    private TextArea endNarrativeField;

    public ScenarioEditor(Scenario scenario)
    {
        CurrentScenario = scenario;
        this.scenario = scenario;

        nameField = new TextField(scenario.name, "Name:", "Name to be displayed in the editor");
        nameField.Changed += (object sender, ControlChangedArgs<string> e) =>
        {
            scenario.name = e.Value;
        };
        foreach (Choice choice in scenario.Choices)
        {
            choiceFoldout.Add(false);
            choiceEditors.Add(new ChoiceEditor(choice, scenario));
        }

        actorsFoldout = new Foldout("Actors");
        actorsFoldout.Style.FontStyle = FontStyle.Bold;
        choicesFoldout = new Foldout("Choices");
        choicesFoldout.Style.FontStyle = FontStyle.Bold;

        endNarrativeLabel = new LabelField("End Narrative:");
        endNarrativeField = new TextArea(scenario.endNarrative);
        endNarrativeField.Changed += (object sender, ControlChangedArgs<string> e) =>
        {
            scenario.endNarrative = e.Value;
        };
    }

    public void Draw()
    {
        CurrentScenario = scenario;

        nameField.Draw();

        actorsFoldout.Draw();
        if (actorsFoldout.Value)
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

        choicesFoldout.Draw();
        if (choicesFoldout.Value)
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

        endNarrativeLabel.Draw();
        endNarrativeField.Draw();
    }
}
