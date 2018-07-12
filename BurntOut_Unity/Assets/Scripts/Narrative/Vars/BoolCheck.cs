using System;

namespace Narrative.Vars
{
    public enum BoolCheckType
    {
        Equals, NotEquals
    }

    [Serializable]
    public class BoolCheck
    {
        public bool value;

        public BoolCheckType checkType;

        public bool Check(bool value)
        {
            switch (checkType)
            {
                case BoolCheckType.Equals:
                    return value == this.value;
                case BoolCheckType.NotEquals:
                    return value != this.value;
                default:
                    return false;
            }
        }
    }
}