using OOEditor;
using System;
using System.Linq;

namespace Narrative.Inspector
{
    public class TaskDrawer : ClassDrawer<Task>
    {
        EnumPopup actionPopup;
        Popup actorPopup;
        TextField dialogueField;
        EnumPopup emotionPopup;
        
        public TaskDrawer(Task value) : base(value)
        {
            actionPopup = new EnumPopup(Value.action)
            {
                FitWidth = true
            };
            actionPopup.Changed += (sender, e) =>
            {
                Value.action = (TaskAction)e.Value;
            };

            Scenario scenario = ScenarioEditor.CurrentScenario;
            actorPopup = new Popup(SceneActors.GetActorIndex(Value.actorId), SceneActors.ActorNames)
            {
                FitWidth = true
            };
            actorPopup.Changed += (sender, e) =>
            {
                if (e.Value >= 0)
                    Value.actorId = SceneActors.GetActorId(actorPopup.Options[e.Value]);
            };

            dialogueField = new TextField(Value.dialogue);
            dialogueField.Changed += (sender, e) =>
            {
                Value.dialogue = e.Value;
            };

            emotionPopup = new EnumPopup(Value.emotion)
            {
                FitWidth = true
            };
            emotionPopup.Changed += (sender, e) =>
            {
                Value.emotion = (TaskEmotion)e.Value;
            };
        }

        protected override void Display()
        {
            actionPopup.Draw(Value.action);

            Scenario scenario = ScenarioEditor.CurrentScenario;
            actorPopup.Options = SceneActors.ActorNames;
            actorPopup.Draw(SceneActors.GetActorIndex(Value.actorId));

            switch (Value.action)
            {
                case TaskAction.TALK:
                    dialogueField.Draw(Value.dialogue);
                    break;
                case TaskAction.EMOTION:
                    emotionPopup.Draw(Value.emotion);
                    break;
            }
        }
    }
}