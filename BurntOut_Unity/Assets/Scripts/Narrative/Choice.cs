using System;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    [Serializable]
    public class Choice
    {
        public string name;
        public string text;

        public bool continueLast = true;
        [SerializeField]
        private List<Trigger> triggers;
        public List<Trigger> Triggers
        {
            get
            {
                if (triggers == null)
                    triggers = new List<Trigger>();

                return triggers;
            }
        }

        [SerializeField]
        private List<Task> events;
        public List<Task> Events
        {
            get
            {
                if (events == null)
                    events = new List<Task>();

                return events;
            }
        }

        public bool isChoice;
        [SerializeField]
        private List<Option> options;
        public List<Option> Options
        {
            get
            {
                if (options == null)
                    options = new List<Option>();

                return options;
            }
        }
    }
}