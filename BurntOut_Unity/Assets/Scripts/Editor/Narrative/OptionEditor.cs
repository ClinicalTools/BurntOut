using CtiEditor;
using OOEditor;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Manages the editing of an option for a given scenario.
/// </summary>
public class OptionEditor
{
    private bool eventsFoldout;
    private readonly Option option;
    private ReorderableList<Task, TaskDrawer> taskList;
    private float feedbackWidth;


    public OptionEditor(Option option, Scenario scenario)
    {
        this.option = option;
        
        taskList = new ReorderableList<Task, TaskDrawer>(option.Events);
    }

    public void Edit()
    {
        option.name = CtiEditorGUI.TextField(option.name, "Name: ", "Name to be displayed in the editor");

        option.text = CtiEditorGUI.TextField(option.text, "Text: ", "Text to be displayed in game");

        using (CtiEditorGUI.LabelFontStyle(FontStyle.Bold))
            eventsFoldout = CtiEditorGUI.Foldout(eventsFoldout, "Events");

        if (eventsFoldout)
            using (CtiEditorGUI.Indent())
                taskList.Draw();

        Color color = new Color();
        if (option.result == OptionResults.CONTINUE)
            color = EditorHelper.ContinueColor;
        else if (option.result == OptionResults.TRY_AGAIN)
            color = EditorHelper.TryAgainColor;
        else if (option.result == OptionResults.END)
            color = EditorHelper.EndColor;
        using (CtiEditorGUI.Color(color))
            option.result = (OptionResults)CtiEditorGUI.EnumPopup(option.result, "Result: ");

        CtiEditorGUI.LabelField("Feedback:");
        option.feedback = CtiEditorGUI.TextArea(option.feedback, ref feedbackWidth);
    }
}
