using System;
using System.Collections.Generic;

namespace OOEditor
{
    public abstract class OOList<T, TDrawer> where TDrawer : IGUIObjectDrawer<T>
    {
        public List<T> Value { get; set; }
        protected List<TDrawer> Drawers { get; } = new List<TDrawer>();

        /// <summary>
        /// Used if the element to initialize shouldn't be initialized with the default constructor.
        /// </summary>
        public Func<T> DefaultElement { get; set; }

        public OOList(List<T> value)
        {
            Value = value;

            for (var i = 0; i < Value.Count; i++)
                AddRow();
        }

        
        protected virtual void SwapRows(int index1, int index2)
        {
            var val = Value[index1];
            Value.RemoveAt(index1);
            Value.Insert(index2, val);
            var draw = Drawers[index1];
            Drawers.RemoveAt(index1);
            Drawers.Insert(index2, draw);
        }

        /// <summary>
        /// Creates a new row
        /// </summary>
        protected virtual void AddRow()
        {
            // Check if we need to add a value with this row
            if (Value.Count == Drawers.Count)
            {
                T val;
                if (DefaultElement != null)
                    val = DefaultElement();
                else
                    val = (T)Activator.CreateInstance(typeof(T));
                Value.Add(val);
            }

            object[] args = { Value[Drawers.Count] };
            var drawer = (TDrawer)Activator.CreateInstance(typeof(TDrawer), args);
            Drawers.Add(drawer);
        }
        /// <summary>
        /// Deletes a row
        /// </summary>
        protected virtual void RemoveRow(int index)
        {
            // Check if we need to remove a value with this row
            if (Value.Count == Drawers.Count)
                Value.RemoveAt(index);
            Drawers.RemoveAt(index);
        }

        public virtual void Draw()
        {
            // Since the list is shared outside this class, values must be constantly updated to avoid errors
            while (Drawers.Count < Value.Count)
                AddRow();
            while (Drawers.Count > Value.Count)
                RemoveRow(Value.Count);
            for (var i = 0; i < Value.Count; i++)
                if (!Drawers[i].Value.Equals(Value[i]))
                    Drawers[i].Value = Value[i];

            Display();

            // This allows value type lists to function correctly
            // Also ensures the drawer is using a valid reference
            for (int i = 0; i < Value.Count; i++)
                if (!Drawers[i].Value.Equals(Value[i]))
                    Value[i] = Drawers[i].Value;
        }

        protected abstract void Display();
    }
}
