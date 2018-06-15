using OOEditor;
using System;

namespace Narrative.Inspector
{
    public class TaskDrawer : ClassDrawer<Task>
    {
        EnumPopup actionPopup;
        Popup actorPopup;
        TextField dialogueField;
        EnumPopup emotionPopup;
        
        public TaskDrawer(Task value)
        {
            Value = value;

            actionPopup = new EnumPopup(value.action)
            {
                Width = 75
            };
            actionPopup.Changed += (object sender, ControlChangedArgs<Enum> e) =>
            {
                Value.action = (TaskAction)e.Value;
            };

            Scenario scenario = ScenarioEditor.CurrentScenario;
            actorPopup = new Popup(scenario.ActorIndex(value.actorId), scenario.ActorNames())
            {
                Width = 90
            };
            actorPopup.Changed += (object sender, ControlChangedArgs<int> e) =>
            {
                if (e.Value >= 0)
                    Value.actorId = scenario.Actors[e.Value].id;
            };

            dialogueField = new TextField(value.dialogue);
            dialogueField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.dialogue = e.Value;
            };

            emotionPopup = new EnumPopup(value.emotion)
            {
                Width = 120
            };
            emotionPopup.Changed += (object sender, ControlChangedArgs<Enum> e) =>
            {
                Value.emotion = (TaskEmotion)e.Value;
            };
        }

        public override void ResetValues()
        {
            actionPopup.Value = Value.action;
            Scenario scenario = ScenarioEditor.CurrentScenario;
            actorPopup.Value = scenario.ActorIndex(Value.actorId);
            dialogueField.Value = Value.dialogue;
            emotionPopup.Value = Value.emotion;
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
}