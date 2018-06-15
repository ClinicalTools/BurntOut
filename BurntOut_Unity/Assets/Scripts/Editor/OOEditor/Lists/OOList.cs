using OOEditor.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public abstract class OOList<T, TDrawer> where TDrawer : IGUIObjectDrawer<T>
    {
        public List<T> List { get; set; }

        public event EventHandler<ListChangedArgs<T>> Changed;

        protected List<TDrawer> Drawers { get; } = new List<TDrawer>();

        /// <summary>
        /// Used if the element to initialize shouldn't be initialized with the default constructor.
        /// </summary>
        public Func<T> DefaultElement { get; set; }

        public OOList(List<T> value)
        {
            List = value;

            for (var i = 0; i < List.Count; i++)
                AddRow();
        }

        protected virtual void SwapRows(int index1, int index2)
        {
            var val = List[index1];
            List.RemoveAt(index1);
            List.Insert(index2, val);
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
            if (List.Count == Drawers.Count)
            {
                T val;
                if (DefaultElement != null)
                    val = DefaultElement();
                else
                    val = (T)Activator.CreateInstance(typeof(T));
                List.Add(val);
            }

            object[] args = { List[Drawers.Count] };
            var drawer = (TDrawer)Activator.CreateInstance(typeof(TDrawer), args);
            Drawers.Add(drawer);
        }
        /// <summary>
        /// Deletes a row
        /// </summary>
        protected virtual void RemoveRow(int index)
        {
            // Check if we need to remove a value with this row
            if (List.Count == Drawers.Count)
                List.RemoveAt(index);
            Drawers.RemoveAt(index);
        }

        private bool firstDraw = true;
        public virtual void Draw()
        {
            // First draw calls changed, since it changed from its default
            if (firstDraw)
            {
                Changed?.Invoke(this, new ListChangedArgs<T>(List));
                firstDraw = false;
            }

            var count = List.Count;

            // Since the list is shared outside this class, values must be constantly updated to avoid errors
            while (Drawers.Count < List.Count)
                AddRow();
            while (Drawers.Count > List.Count)
                RemoveRow(List.Count);
            for (var i = 0; i < List.Count; i++)
                if (!Drawers[i].Value.Equals(List[i]))
                    Drawers[i].Value = List[i];

            // Listen to whether contained elements change
            OOEditorManager.Changed += OnChanged;
            Display();
            OOEditorManager.Changed -= OnChanged;

            // This allows value type lists to function correctly
            // Also ensures the drawer is using a valid reference
            for (int i = 0; i < List.Count; i++)
                if (!Drawers[i].Value.Equals(List[i]))
                    List[i] = Drawers[i].Value;

            if (count != List.Count)
            {
                var e = new ListChangedArgs<T>(List);
                OOEditorManager.ElementChanged(this, e);
                Changed?.Invoke(this, e);
            }
        }
        /// <summary>
        /// Updates the list's reference and then draws it.
        /// </summary>
        /// <param name="list">Reference of list to draw</param>
        public void Draw(List<T> list)
        {
            List = list;

            Draw();
        }

        protected void OnChanged(object sender, EventArgs e)
        {
            Changed?.Invoke(this, new ListChangedArgs<T>(List));
        }

        protected abstract void Display();
    }
}
