using OOEditor;
using System;

public class TaskDrawer : IGUIObjectDrawer<Task>
{
    EnumPopup actionPopup;
    Popup actorPopup;
    TextField dialogueField;
    EnumPopup emotionPopup;

    private Task value;
    public Task Value
    {
        get
        {
            return value;
        }
        set
        {
            this.value = value;
            actionPopup.Value = value.action;
            Scenario scenario = ScenarioEditor.CurrentScenario;
            actorPopup.Value = scenario.ActorIndex(value.actorId);
            dialogueField.Value = value.dialogue;
            emotionPopup.Value = value.emotion;
        }
    }

    public TaskDrawer(Task val)
    {
        value = val;

        actionPopup = new EnumPopup(val.action);
        actionPopup.Changed += (object sender, ControlChangedArgs<Enum> e) =>
        {
            Value.action = (TaskAction)e.Value;
        };
        actionPopup.Width = 75;

        Scenario scenario = ScenarioEditor.CurrentScenario;
        actorPopup = new Popup(scenario.ActorIndex(val.actorId), scenario.ActorNames());
        actorPopup.Changed += (object sender, ControlChangedArgs<int> e) =>
        {
            Value.actorId = scenario.Actors[e.Value].id;
        };
        actorPopup.Width = 90;

        dialogueField = new TextField(val.dialogue);
        dialogueField.Changed += (object sender, ControlChangedArgs<string> e) =>
        {
            Value.dialogue = e.Value;
        };

        emotionPopup = new EnumPopup(val.emotion);
        emotionPopup.Changed += (object sender, ControlChangedArgs<Enum> e) =>
        {
            Value.emotion = (TaskEmotion)e.Value;
        };
        emotionPopup.Width = 120;
    }

    public void Draw()
    {
        actionPopup.Draw();

        actorPopup.Options = ScenarioEditor.CurrentScenario.ActorNames();
        actorPopup.Draw();

        switch (Value.action)
        {
            case TaskAction.TALK:
                dialogueField.Draw();
                break;
            case TaskAction.EMOTION:
                emotionPopup.Draw();
                break;
        }
    }
}
