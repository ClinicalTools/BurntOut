using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Narrative
{
    [Serializable]
    public class Scenario
    {
        // Number to represent this actor.
        public int id;

        public string scenePath;
        public bool sceneChange;
        public bool autoChangeScene = true;
        [SerializeField]
        private List<Trigger> sceneChangeTriggers;
        public List<Trigger> SceneChangeTriggers
        {
            get
            {
                if (sceneChangeTriggers == null)
                    sceneChangeTriggers = new List<Trigger>();

                return sceneChangeTriggers;
            }
        }

        [SerializeField]
        private List<Choice> choices;
        public List<Choice> Choices
        {
            get
            {
                if (choices == null)
                    choices = new List<Choice>();

                return choices;
            }
        }


        [SerializeField]
        private List<ActorEvents> actorEventsList;
        public List<ActorEvents> ActorEventsList
        {
            get
            {
                if (actorEventsList == null)
                    actorEventsList = new List<ActorEvents>();

                return actorEventsList;
            }
        }

        public bool hasStartNarrative;
        public string startNarrative;

        public bool hasEndNarrative;
        public string endNarrative;

        public Scenario(Scenario[] scenarios)
        {
            ResetHash(scenarios);
        }

        public void ResetHash(Scenario[] scenarios)
        {
            // Using hash of current time to get a number that will hopefully be unique
            var hash = DateTime.Now.GetHashCode();

            // Ensure no other scenario in the passed list has the same id
            // a true flag means that the code hasn't hit another scenario with the same id
            bool flag = false;
            while (!flag)
            {
                flag = true;
                foreach (var actor in scenarios)
                    if (actor.id == hash)
                    {
                        hash++;
                        flag = false;
                        break;
                    }
            }

            id = hash;
        }
    }
}