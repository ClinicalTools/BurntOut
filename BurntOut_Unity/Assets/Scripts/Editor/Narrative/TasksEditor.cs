using CtiEditor;
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
        EditorStyles.popup.fixedHeight = 0;
        list.drawElementCallback =
            (Rect rect, int index, bool isActive, bool isFocused) =>
            {
                var element = (Task)list.list[index];
                rect.y += 1;
                rect.height -= 4;
                Rect newRect = new Rect(rect.x, rect.y, (EditorStyles.popup.fontSize + 4) * 5.1f, rect.height);
                element.action = (TaskAction)EditorGUI.EnumPopup(newRect, element.action);
                rect.width -= newRect.width + 5;
                rect.x += newRect.width + 5;

                newRect = new Rect(rect.x, rect.y, (EditorStyles.popup.fontSize + 4) * 6f, rect.height);

                var actorIndex = EditorGUI.Popup(newRect, scenario.ActorIndex(element.actorId), scenario.ActorNames());
                if (actorIndex != -1)
                    element.actorId = scenario.Actors[actorIndex].id;

                rect.width -= newRect.width + 5;
                rect.x += newRect.width + 5;

                switch (element.action)
                {
                    case TaskAction.TALK:
                        newRect = new Rect(rect.x, rect.y, rect.width, rect.height);
                        element.dialogue = EditorGUI.TextField(newRect, element.dialogue);
                        break;
                    case TaskAction.EMOTION:
                        newRect = new Rect(rect.x, rect.y, 120, rect.height);
                        element.emotion = (TaskEmotion)EditorGUI.EnumPopup(newRect, element.emotion);
                        break;
                }
            };
    }

    public void Edit()
    {
        list.elementHeight = EditorStyles.popup.CalcHeight(new GUIContent(" "), 100) + 4;
        list.headerHeight = 4;
        using (CtiEditorGUI.Vertical())
            list.DoLayoutList();
    }
}
