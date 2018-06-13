using System;

namespace OOEditor
{
    public class ControlChangedArgs<T> : EventArgs
    {
        public T LastValue { get; }
        public T Value { get; }

        public ControlChangedArgs(T lastValue, T value)
        {
            LastValue = lastValue;
            Value = value;
        }
    }
}