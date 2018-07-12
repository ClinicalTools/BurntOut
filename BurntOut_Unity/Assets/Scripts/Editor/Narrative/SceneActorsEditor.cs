using OOEditor;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of a scenario.
    /// </summary>
    public class SceneActorsEditor : ClassDrawer<Scenario>
    {
        private readonly Button editActorsBtn;

        private readonly FoldoutList<ActorEvents, ActorEventsEditor> actorEvents;
        private readonly List<ActorEvents> actorEventsList = new List<ActorEvents>();

        public SceneActorsEditor(Scenario value) : base(value)
        {
            editActorsBtn = new Button("Edit Actor Prefabs");
            editActorsBtn.Style.FontSize = 14;
            editActorsBtn.Style.FontStyle = FontStyle.Bold;
            editActorsBtn.Pressed += (sender, e) =>
            {
                ActorPrefabsEditorWindow.Init();
            };

            UpdateActorEventsList();
            actorEvents = new FoldoutList<ActorEvents, ActorEventsEditor>(actorEventsList, 
                false, false, false);
        }

        private void UpdateActorEventsList()
        {
            actorEventsList.Clear();
            foreach (var actor in SceneActors.Actors)
            {
                var actorEvents = Value.ActorEventsList.FirstOrDefault(
                    a => a.actorId == actor.id);
                if (actorEvents == null)
                {
                    actorEvents = new ActorEvents()
                    {
                        actorId = actor.id
                    };
                    Value.ActorEventsList.Add(actorEvents);
                }
                actorEventsList.Add(actorEvents);
            }
        }


        protected override void Display()
        {
            editActorsBtn.Draw();

            actorEvents.Draw(actorEventsList);
        }
    }
}