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
        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        scenario.Name = EditorGUILayout.TextField(new GUIContent("Name: ", "Name to be displayed in the editor"), scenario.Name);


        actorsFoldout = EditorGUILayout.Foldout(actorsFoldout, "Actors", EditorStyles.foldout);
        if (actorsFoldout)
        {
            using (new EditorIndent())
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
                        scenario.Actors[i].name = EditorGUILayout.TextField(scenario.Actors[i].name);
                    },
                    "Remove Actor",
                    "Are you sure you want to delete this actor?"
                );
            }
        }


        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        choicesFoldout = EditorGUILayout.Foldout(choicesFoldout, "Choices", EditorStyles.foldout);
        EditorStyles.foldout.fontStyle = FontStyle.Normal;
        if (choicesFoldout)
        {
            using (new EditorIndent())
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
                        using (new EditorVertical(EditorStyles.helpBox))
                            choiceEditors[i].Edit();
                    },
                    choiceFoldout,
                    // Foldout title
                    (int i) => { return ("Choice " + (i + 1) + " - " + scenario.Choices[i].Name); },
                    // Foldout color
                    (int i) =>
                    {
                        // Red if there is no option to continue
                        foreach (Option option in scenario.Choices[i].Options)
                            if (option.Result == OptionResults.CONTINUE)
                                return GUI.contentColor;

                        return EditorHelper.ErrorColor;
                    },
                    "Remove Choice",
                    "Are you sure you want to delete this choice?"
                );
            }
        }


        EditorGUILayout.LabelField("End Narrative:");
        EditorStyles.textField.wordWrap = true;
        scenario.EndNarrative = EditorGUILayout.TextArea(scenario.EndNarrative);
    }
}
