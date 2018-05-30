using CtiEditor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Manages the editing of an option for a given scenario.
/// </summary>
public class OptionEditor
{
    private bool eventsFoldout;
    private readonly Option option;
    private readonly TasksEditor tasksEditor;
    private float feedbackWidth;

    public OptionEditor(Option option, Scenario scenario)
    {
        this.option = option;

        tasksEditor = new TasksEditor(option.Events, scenario);
    }

    public void Edit()
    {
        option.name = CtiEditorGUI.TextField(option.name, "Name: ", "Name to be displayed in the editor");

        option.text = CtiEditorGUI.TextField(option.text, "Text: ", "Text to be displayed in game");

        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        eventsFoldout = CtiEditorGUI.Foldout(eventsFoldout, "Events");
        EditorStyles.foldout.fontStyle = FontStyle.Normal;

        if (eventsFoldout)
            using (new EditorIndent())
                using (new EditorVertical())
                    tasksEditor.Edit();

        var lastColor = GUI.contentColor;
        if (option.result == OptionResults.CONTINUE)
            GUI.contentColor = EditorHelper.ContinueColor;
        else if (option.result == OptionResults.TRY_AGAIN)
            GUI.contentColor = EditorHelper.TryAgainColor;
        else if (option.result == OptionResults.END)
            GUI.contentColor = EditorHelper.EndColor;
        option.result = (OptionResults)CtiEditorGUI.EnumPopup(option.result, "Result: ");
        GUI.contentColor = lastColor;

        CtiEditorGUI.LabelField("Feedback:");
        option.feedback = CtiEditorGUI.TextArea(option.feedback, ref feedbackWidth);
    }
}
