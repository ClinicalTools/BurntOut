using System;

namespace Narrative
{
    public enum TriggerType { ENTER, TALK, INTERACT}

    [Serializable]
    public class Trigger
    {
        public TriggerType type;

        public int id;
    }
}