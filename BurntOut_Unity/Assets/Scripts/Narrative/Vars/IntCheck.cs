using System;

namespace Narrative.Vars
{
    public enum IntCheckType
    {
        Equals, NotEquals, LessThan, GreaterThan, LessThanOrEquals, GreaterThanOrEquals
    }

    [Serializable]
    public class IntCheck
    {
        public int value;

        public IntCheckType checkType;

        public bool Check(int value)
        {
            switch (checkType)
            {
                case IntCheckType.Equals:
                    return value == this.value;
                case IntCheckType.NotEquals:
                    return value != this.value;
                case IntCheckType.LessThan:
                    return value < this.value;
                case IntCheckType.GreaterThan:
                    return value > this.value;
                case IntCheckType.LessThanOrEquals:
                    return value <= this.value;
                case IntCheckType.GreaterThanOrEquals:
                    return value >= this.value;
                default:
                    return false;
            }
        }
    }
}