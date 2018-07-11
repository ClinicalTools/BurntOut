using OOEditor;

namespace Narrative.Inspector
{
    public class TriggerDrawer : ClassDrawer<Trigger>
    {
        private readonly EnumPopup typePopup;
        private readonly Popup actorPopup, locationPopup, interactablePopup;

        public TriggerDrawer(Trigger value) : base(value)
        {
            typePopup = new EnumPopup(value.type)
            {
                FitWidth = true
            };
            typePopup.Changed += (sender, e) =>
            {
                Value.type = (TriggerType)e.Value;
            };

            actorPopup = new Popup(SceneActors.GetActorIndex(Value.id), SceneActors.NpcNames)
            {
                FitWidth = true
            };
            actorPopup.Changed += (sender, e) =>
            {
                if (e.Value >= 0)
                    Value.id = SceneActors.GetActorId(actorPopup.Options[e.Value]);
            };

            interactablePopup = new Popup(SceneInteractables.GetIndex(Value.interactable),
                SceneInteractables.Names)
            {
                FitWidth = true
            };
            interactablePopup.Changed += (sender, e) =>
            {
                if (e.Value >= 0)
                {
                    Value.interactable = SceneInteractables.GetInteractable(
                        interactablePopup.Options[e.Value]);
                }
            };
        }

        protected override void Display()
        {
            typePopup.Draw(Value.type);

            switch (Value.type)
            {
                case TriggerType.Enter:
                    break;
                case TriggerType.Interact:
                    interactablePopup.Options = SceneInteractables.Names;
                    interactablePopup.Draw(SceneInteractables.GetIndex(Value.interactable));
                    break;
                case TriggerType.Talk:
                    actorPopup.Options = SceneActors.NpcNames;
                    actorPopup.Draw(SceneActors.GetNpcIndex(Value.id));
                    break;
            }
        }
    }
}