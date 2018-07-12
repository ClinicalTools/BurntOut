using Narrative.Vars;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Narrative
{
    [Serializable]
    public class Scenario
    {
        // Basic narrative info
        public bool hasStartNarrative;
        public string startNarrative;

        public bool hasEndNarrative;
        public string endNarrative;

        // Variable info
        [SerializeField]
        private List<NarrativeVar> vars;
        public List<NarrativeVar> Vars
        {
            get
            {
                if (vars == null)
                    vars = new List<NarrativeVar>();

                return vars;
            }
        }

        // Actor info
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

        // Scene change info
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

        // Event info
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
    }
}