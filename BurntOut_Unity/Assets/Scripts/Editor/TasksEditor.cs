using System.Collections;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

/// <summary>
/// Manages the editing of a list of tasks for a given scenario.
/// </summary>
public class TasksEditor
{
    private ReorderableList list;

    public TasksEditor(IList tasks, Scenario scenario)
    {
        list = new ReorderableList(tasks, typeof(Task));

        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = (Task)list.list[index];
                rect.y += 2;

                Rect newRect = new Rect(rect.x, rect.y, 75, EditorGUIUtility.singleLineHeight);
                element.action = (TaskAction)EditorGUI.EnumPopup(newRect, element.action);
                rect.width -= newRect.width + 5;
                rect.x += newRect.width + 5;

                newRect = new Rect(rect.x, rect.y, 90, EditorGUIUtility.singleLineHeight);
                element.actor = EditorGUI.Popup(newRect, element.actor, scenario.Actors.ToArray());
                rect.width -= newRect.width + 5;
                rect.x += newRect.width + 5;

                switch (element.action)
                {
                    case TaskAction.TALK:

                        newRect = new Rect(rect.x, rect.y, rect.width, EditorGUIUtility.singleLineHeight);
                        element.dialogue = EditorGUI.TextField(newRect, element.dialogue);
                        break;
                    case TaskAction.EMOTION:
                        newRect = new Rect(rect.x, rect.y, 90, EditorGUIUtility.singleLineHeight);
                        element.emotion = (TaskEmotion)EditorGUI.EnumPopup(newRect, element.emotion);
                        break;
                }
            };
    }

    public void Edit()
    {
        list.DoLayoutList();
    }
}
