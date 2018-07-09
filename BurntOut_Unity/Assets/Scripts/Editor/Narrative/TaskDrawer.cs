using OOEditor;

namespace Narrative.Inspector
{
    public class TaskDrawer : ClassDrawer<Task>
    {
        private readonly EnumPopup typePopup;
        // Dialogue
        private readonly Popup actorPopup;
        private readonly TextField dialogueField;

        // Emotion
        private readonly EnumPopup emotionPopup;
        private readonly Popup npcPopup;

        // Action
        private readonly EnumPopup actionPopup;
        private readonly Popup positionPopup, activatablePopup;

        public TaskDrawer(Task value) : base(value)
        {
            typePopup = new EnumPopup(Value.type)
            {
                FitWidth = true
            };
            typePopup.Changed += (sender, e) =>
            {
                Value.type = (TaskType)e.Value;
            };

            // Dialogue
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

            // Emotion
            emotionPopup = new EnumPopup(Value.emotion)
            {
                FitWidth = true
            };
            emotionPopup.Changed += (sender, e) =>
            {
                Value.emotion = (TaskEmotion)e.Value;
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

            // Action
            actionPopup = new EnumPopup(Value.action)
            {
                FitWidth = true
            };
            actionPopup.Changed += (sender, e) =>
            {
                Value.action = (TaskAction)e.Value;
            };
            positionPopup = new Popup(ScenePositions.GetIndex(Value.position),
                ScenePositions.PositionNames)
            {
                FitWidth = true
            };
            positionPopup.Changed += (sender, e) =>
            {
                if (e.Value >= 0)
                    Value.position = ScenePositions.GetPositions(positionPopup.Options[e.Value]);
            };
            activatablePopup = new Popup(SceneActivatables.GetIndex(Value.activatable),
                SceneActivatables.ActivatableNames)
            {
                FitWidth = true
            };
            activatablePopup.Changed += (sender, e) =>
            {
                if (e.Value >= 0)
                {
                    Value.activatable = SceneActivatables.GetActivatable(
                        activatablePopup.Options[e.Value]);
                }
            };
        }

        protected override void Display()
        {
            typePopup.Draw(Value.type);
            actorPopup.Options = SceneActors.ActorNames;

            switch (Value.type)
            {
                case TaskType.Talk:
                    actorPopup.Options = SceneActors.ActorNames;
                    actorPopup.Draw(SceneActors.GetActorIndex(Value.actorId));
                    dialogueField.Draw(Value.dialogue);
                    break;
                case TaskType.Emotion:
                    npcPopup.Options = SceneActors.NpcNames;
                    npcPopup.Draw(SceneActors.GetNpcIndex(Value.actorId));
                    emotionPopup.Draw(Value.emotion);
                    break;
                case TaskType.Action:
                    actionPopup.Draw(Value.action);
                    switch (Value.action)
                    {
                        case TaskAction.MoveTo:
                            positionPopup.Options = ScenePositions.PositionNames;
                            positionPopup.Draw(ScenePositions.GetIndex(Value.position));
                            break;
                        case TaskAction.Show:
                            activatablePopup.Options = SceneActivatables.ActivatableNames;
                            activatablePopup.Draw(
                                SceneActivatables.GetIndex(Value.activatable));
                            break;
                        case TaskAction.Hide:
                            activatablePopup.Options = SceneActivatables.ActivatableNames;
                            activatablePopup.Draw(SceneActivatables.GetIndex(Value.activatable));
                            break;
                    }

                    break;
            }
        }
    }
}