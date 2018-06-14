using System;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public abstract class OOList<T, TDrawer> where TDrawer : IGUIObjectDrawer<T>
    {
        public List<T> Value { get; set; }

        public event EventHandler<ListChangedArgs<T>> Changed;

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
                Debug.Log(Value.ToArray().GetHashCode());
                T val;
                if (DefaultElement != null)
                    val = DefaultElement();
                else
                    val = (T)Activator.CreateInstance(typeof(T));
                Value.Add(val);
                Debug.Log(Value.ToArray().GetHashCode());
            }

            object[] args = { Value[Drawers.Count] };
            var drawer = (TDrawer)Activator.CreateInstance(typeof(TDrawer), args);
            drawer.Changed += (object sender, ControlChangedArgs<T> e) =>
            {
                Changed?.Invoke(this, new ListChangedArgs<T>(Value));
            };
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

        private bool firstDraw = true;
        public virtual void Draw()
        {
            // First draw calls changed, since it changed from its default
            if (firstDraw)
            {
                Changed?.Invoke(this, new ListChangedArgs<T>(Value));
                firstDraw = false;
            }

            var count = Value.Count;

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

            if (count != Value.Count)
                Changed?.Invoke(this, new ListChangedArgs<T>(Value));
        }

        protected abstract void Display();
    }
}
