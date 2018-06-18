using System;
using System.Collections.Generic;

namespace OOEditor
{

    /// <summary>
    /// Arguments used for editor lists' changed events.
    /// </summary>
    /// <typeparam name="T">Type of value in the editor list's list</typeparam>
    public class ListChangedArgs<T> : EventArgs
    {
        /// <summary>
        /// List managed by the editor list.
        /// </summary>
        public List<T> Value { get; }

        /// <summary>
        /// Creates arguments to be used for a editor list's changed event.
        /// </summary>
        /// <param name="value">Current value held by the editor list</param>
        public ListChangedArgs(List<T> value)
        {
            Value = value;
        }
    }
}