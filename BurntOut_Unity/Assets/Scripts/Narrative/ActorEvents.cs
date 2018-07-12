using System;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    [Serializable]
    public class ActorEvents
    {
        public int actorId;

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
    }
}
