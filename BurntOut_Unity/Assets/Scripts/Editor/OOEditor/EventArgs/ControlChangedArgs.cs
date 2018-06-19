using System;

namespace OOEditor
{
    /// <summary>
    /// Arguments used for controls' changed events.
    /// </summary>
    /// <typeparam name="T">Type of the control's value</typeparam>
    public class ControlChangedArgs<T> : EventArgs
    {
        public T LastValue { get; }
        public T Value { get; }

        /// <summary>
        /// Creates arguments to be used for a control's changed event.
        /// </summary>
        /// <param name="lastValue">Previous value held by the control.</param>
        /// <param name="value">Current value held by the control.</param>
        public ControlChangedArgs(T lastValue, T value)
        {
            LastValue = lastValue;
            Value = value;
        }
    }
}