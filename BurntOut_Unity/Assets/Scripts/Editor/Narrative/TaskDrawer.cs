using OOEditor;
using System;

public class TaskDrawer : GUIObjectDrawer<Task>
{
    EnumPopup actionPopup;
    Popup actorPopup;
    TextField dialogueField;
    EnumPopup emotionPopup;

    public override void Init(Task val)
    {
        Value = val;
        actionPopup = new EnumPopup(val.action);
        actionPopup.Changed += (object sender, ControlChangedArgs<Enum> e) =>
        {
            Value.action = (TaskAction)e.Value;
        };
        actionPopup.Width = 75;

        Scenario scenario = ScenarioEditor.CurrentScenario;
        actorPopup = new Popup(scenario.ActorIndex(val.actorId), scenario.ActorNames());
        actorPopup.Changed += (object sener, ControlChangedArgs<int> e) =>
        {
            Value.actorId = scenario.Actors[e.Value].id;
        };
        actorPopup.Width = 90;

        dialogueField = new TextField(val.dialogue);
        dialogueField.Changed += (object sener, ControlChangedArgs<string> e) =>
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

    public override void Draw()
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
