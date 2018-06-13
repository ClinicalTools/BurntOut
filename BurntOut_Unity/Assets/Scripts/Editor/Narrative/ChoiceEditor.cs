using CtiEditor;
using OOEditor;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the editing of a choice for a given scenario.
/// </summary>
public class ChoiceEditor : FoldoutObjectDrawer<Choice>
{
    protected override string FoldoutName => $"Choice {ScenarioEditor.CurrentScenario.Choices.IndexOf(Value) + 1} - {Value.name}";
    protected override Color? FoldoutColor
    {
        get
        {
            if (Value.Options.Exists(option => option.result == OptionResults.CONTINUE))
                return null;
            else
                return EditorColors.Red;
        }
    }

    public static Choice CurrentChoice { get; private set; }

    private readonly TextField nameField;
    private readonly Foldout eventsFoldout;
    private readonly ReorderableList<Task, TaskDrawer> taskList;
    private readonly TextField textField;
    private readonly Foldout optionsFoldout;
    private readonly FoldoutList<Option, OptionEditor> optionList;

    //private bool eventsFoldout;
    //private bool optionsFoldout;
    private readonly List<bool> optionFoldout = new List<bool>();
    private readonly List<OptionEditor> optionEditors = new List<OptionEditor>();
    
    public ChoiceEditor(Choice choice)
    {
        CurrentChoice = choice;
        Value = choice;

        int choiceNum = ScenarioEditor.CurrentScenario.Choices.IndexOf(Value);

        nameField = new TextField(choice.name, "Name:", "Name to be displayed in the editor");
        nameField.Changed += (object sender, ControlChangedArgs<string> e) =>
        {
            choice.name = e.Value;
        };

        textField = new TextField(choice.text, "Text:", "Text to be displayed in game");
        textField.Changed += (object sender, ControlChangedArgs<string> e) =>
        {
            choice.text = e.Value;
        };

        eventsFoldout = new Foldout(false, "Events");
        eventsFoldout.Style.FontStyle = FontStyle.Bold;
        taskList = new ReorderableList<Task, TaskDrawer>(Value.Events);

        optionsFoldout = new Foldout(false, "Options");
        optionsFoldout.Style.FontStyle = FontStyle.Bold;
        optionList = new FoldoutList<Option, OptionEditor>(Value.Options);
    }

    public override void Draw()
    {
        CurrentChoice = Value;

        nameField.Draw();

        eventsFoldout.Draw();
        if (eventsFoldout.Value)
            using (CtiEditorGUI.Indent())
                taskList.Draw();

        textField.Draw();

        optionsFoldout.Draw();
        if (optionsFoldout.Value)
            using (CtiEditorGUI.Indent())
                optionList.Draw();
    }
}
