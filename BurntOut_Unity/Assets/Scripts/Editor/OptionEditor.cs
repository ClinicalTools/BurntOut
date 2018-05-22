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


    public OptionEditor(Option option, Scenario scenario)
    {
        this.option = option;

        tasksEditor = new TasksEditor(option.Events, scenario);
    }

    public void Edit()
    {
        option.Name = EditorGUILayout.TextField(new GUIContent("Name: ", "Name to be displayed in the editor"), option.Name);

        option.Text = EditorGUILayout.TextField(new GUIContent("Text: ", "Text to be displayed in game"), option.Text);

        EditorStyles.foldout.fontStyle = FontStyle.Bold;
        eventsFoldout = EditorGUILayout.Foldout(eventsFoldout, "Events", EditorStyles.foldout);
        EditorStyles.foldout.fontStyle = FontStyle.Normal;

        if (eventsFoldout)
        {
            tasksEditor.Edit();
        }

        var lastColor = GUI.contentColor;
        if (option.Result == OptionResults.CONTINUE)
            GUI.contentColor = EditorHelper.ContinueColor;
        else if (option.Result == OptionResults.TRY_AGAIN)
            GUI.contentColor = EditorHelper.TryAgainColor;
        else if (option.Result == OptionResults.END)
            GUI.contentColor = EditorHelper.EndColor;
        option.Result = (OptionResults)EditorGUILayout.EnumPopup("Result: ", option.Result);
        GUI.contentColor = lastColor;

        EditorGUILayout.LabelField("Feedback:");
        EditorStyles.textField.wordWrap = true;
        option.Feedback = EditorGUILayout.TextArea(option.Feedback);
    }
}
