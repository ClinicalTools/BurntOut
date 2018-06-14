using System;
using System.Collections.Generic;

namespace OOEditor
{
    public class ListChangedArgs<T> : EventArgs
    {
        public List<T> Value { get; }

        public ListChangedArgs(List<T> value)
        {
            Value = value;
        }
    }
}