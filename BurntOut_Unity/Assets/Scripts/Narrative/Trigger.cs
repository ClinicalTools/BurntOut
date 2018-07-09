using System;

namespace Narrative
{
    public enum TriggerType { Enter, Talk, Interact}

    [Serializable]
    public class Trigger
    {
        public TriggerType type;

        public int id;
    }
}