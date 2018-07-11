using System;

namespace Narrative
{
    public enum TriggerType { Enter, Talk, Interact}

    [Serializable]
    public class Trigger
    {
        public TriggerType type;

        // Used for triggers with actors
        public int id;

        // Used for triggers with interactable objects
        public Interactable interactable;
    }
}