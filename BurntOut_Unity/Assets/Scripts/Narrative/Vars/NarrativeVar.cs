using System;

namespace Narrative.Vars
{
    public enum VarType { Bool, Int, String }

    [Serializable]
    public class NarrativeVar
    {
        // Using hash of current time to get a number that will hopefully be unique
        public int id = DateTime.Now.GetHashCode();

        public string name;
        public bool global;

        public VarType type;

        public bool boolVal;
        public int intVal;
        public string stringVal;
    }
}
