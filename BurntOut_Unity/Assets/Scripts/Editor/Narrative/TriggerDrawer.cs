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

            UpdateSceneActors();
            actorPopup = new Popup(GetActorIndex(Value.id), ActorNames())
            {
                FitWidth = true
            };
            actorPopup.Changed += (sender, e) =>
            {
                if (e.Value >= 0)
                    Value.id = GetActorId(actorPopup.Options[e.Value]);
            };
        }

        private int GetActorIndex(int actorId)
        {
            var index = -1;
            for (int i = 0; i < sceneActors.Count; i++)
                if (sceneActors[i].id == Value.id)
                    index = i;

            return index;
        }

        private int GetActorId(string actorName)
        {
            return sceneActors.Find(a => a.name == actorName)?.id ?? 0;
        }

        private void UpdateSceneActors()
        {
            sceneActors.Clear();

            var actorObjs = Object.FindObjectsOfType<ActorObject>().Where(a => a.actor != null);
            if (actorObjs.Count() != sceneActors.Count)
            {
                sceneActors.Clear();
                foreach (var actorObj in actorObjs)
                    sceneActors.Add(actorObj.actor);

                if (actorPopup != null)
                    actorPopup.Options = ActorNames();
            }
            else
            {
                var i = 0;
                foreach (var actorObj in actorObjs)
                    if (actorPopup.Options[i] != actorObj.actor.name)
                        actorPopup.Options[i] = actorObj.actor.name;
            }
        }

        private string[] ActorNames()
        {
            var actorNames = new string[sceneActors.Count];
            for (var i = 0; i < sceneActors.Count; i++)
                actorNames[i] = sceneActors[i].name;

            return actorNames;
        }

        protected override void Display()
        {
            typePopup.Draw(Value.type);

            switch (Value.type)
            {
                case TriggerType.ENTER:
                    break;
                case TriggerType.INTERACT:
                    break;
                case TriggerType.TALK:
                    UpdateSceneActors();
                    actorPopup.Draw(GetActorIndex(Value.id));
                    break;
            }
        }
    }
}