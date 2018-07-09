using OOEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Narrative.Inspector
{
    public class TriggerDrawer : ClassDrawer<Trigger>
    {
        private readonly EnumPopup typePopup;
        private readonly Popup actorPopup, locationPopup, objectPopup;

        List<Actor> sceneActors = new List<Actor>();

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

            actorPopup = new Popup(GetActorIndex(Value.id), SceneActors.NpcNames)
            {
                FitWidth = true
            };
            actorPopup.Changed += (sender, e) =>
            {
                if (e.Value >= 0)
                    Value.id = SceneActors.GetActorId(actorPopup.Options[e.Value]);
            };
        }

        private int GetActorIndex(int actorId)
        {
            var index = -1;
            for (int i = 0; i < sceneActors.Count; i++)
                if (sceneActors[i].id == actorId)
                    index = i;

            return index;
        }

        protected override void Display()
        {
            typePopup.Draw(Value.type);

            switch (Value.type)
            {
                case TriggerType.Enter:
                    break;
                case TriggerType.Interact:
                    break;
                case TriggerType.Talk:
                    actorPopup.Options = SceneActors.NpcNames;
                    actorPopup.Draw(SceneActors.GetNpcIndex(Value.id));
                    break;
            }
        }
    }
}