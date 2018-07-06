using OOEditor;
using System;
using System.Linq;

namespace Narrative.Inspector
{
    public class TaskDrawer : ClassDrawer<Task>
    {
        private readonly EnumPopup actionPopup;
        private readonly Popup actorPopup, npcPopup;
        private readonly TextField dialogueField;
        private readonly EnumPopup emotionPopup;
        
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
            npcPopup = new Popup(SceneActors.GetNpcIndex(Value.actorId), SceneActors.NpcNames)
            {
                FitWidth = true
            };
            npcPopup.Changed += (sender, e) =>
            {
                if (e.Value >= 0)
                    Value.actorId = SceneActors.GetActorId(npcPopup.Options[e.Value]);
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

            switch (Value.action)
            {
                case TaskAction.TALK:
                    actorPopup.Draw(SceneActors.GetActorIndex(Value.actorId));
                    dialogueField.Draw(Value.dialogue);
                    break;
                case TaskAction.EMOTION:
                    npcPopup.Draw(SceneActors.GetNpcIndex(Value.actorId));
                    emotionPopup.Draw(Value.emotion);
                    break;
            }
        }
    }
}