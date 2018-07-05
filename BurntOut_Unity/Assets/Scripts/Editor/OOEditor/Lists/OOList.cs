using OOEditor.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Handles the basic parts of an editor GUI list.
    /// </summary>
    /// <typeparam name="T">Type of element to draw</typeparam>
    /// <typeparam name="TDrawer">Drawer type to use to draw each element</typeparam>
    /// <remarks>
    /// Opted to keep this in the Lists folder, rather than the abstract folder, 
    /// as there's so few lists, and all rely on this file. 
    /// </remarks>
    public abstract class OOList<T, TDrawer> where TDrawer : IGUIObjectDrawer<T>
    {
        /// <summary>
        /// List to manage
        /// </summary>
        public List<T> List { get; set; }
        /// <summary>
        /// List of drawers to draw the elements in the stored list.
        /// </summary>
        protected List<TDrawer> Drawers { get; } = new List<TDrawer>();

        /// <summary>
        /// Occurs when the list is changed.
        /// </summary>
        public event EventHandler<ListChangedArgs<T>> Changed;


        /// <summary>
        /// Used if the element to initialize shouldn't be initialized with the default constructor.
        /// </summary>
        public Func<T> DefaultElement { get; set; }


        /// <summary>
        /// Creates a new OOList to display the values in the passed list.
        /// </summary>
        /// <param name="value">List of values to display.</param>
        public OOList(List<T> value)
        {
            List = value;

            for (var i = 0; i < List.Count; i++)
                AddRow();
        }

        // Used to ensure the correct control is selected after rows swap
        private string setFocusedControl;
        /// <summary>
        /// Swaps the values and drawers at the passed indices.
        /// </summary>
        protected virtual void SwapRows(int index1, int index2)
        {
            var focusedControl = GUI.GetNameOfFocusedControl();
            
            var val = List[index1];
            List.RemoveAt(index1);
            List.Insert(index2, val);
            var draw = Drawers[index1];
            Drawers.RemoveAt(index1);
            Drawers.Insert(index2, draw);

            // Ensure rearranging didn't change the focus
            setFocusedControl = focusedControl;
        }

        /// <summary>
        /// Creates a new row. 
        /// If there are the same number of drawers as elements, also creates a new element.
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
        /// Removes a row and the corresponding element at a given index.
        /// </summary>
        /// <param name="index">Index to remove at.</param>
        protected virtual void RemoveRow(int index)
        {
            // Check if we need to remove a value with this row
            if (List.Count == Drawers.Count)
                List.RemoveAt(index);
            Drawers.RemoveAt(index);
        }

        /// <summary>
        /// Draws the list.
        /// </summary>
        public virtual void Draw()
        {
            var count = List.Count;

            // Since the list is shared outside this class, values must be constantly updated to avoid errors
            while (Drawers.Count < List.Count)
                AddRow();
            while (Drawers.Count > List.Count)
                RemoveRow(List.Count);
            for (var i = 0; i < List.Count; i++)
            {
                if (!Drawers[i].Value.Equals(List[i]))
                {
                    object[] args = { List[i] };
                    Drawers[i] = (TDrawer)Activator.CreateInstance(typeof(TDrawer), args);
                }
            }

            // Ensures element that was previously selected if it swapped positions
            if (setFocusedControl != null)
            {
                GUI.FocusControl(setFocusedControl);
                setFocusedControl = null;
            }

            // Listen to whether contained elements change
            OOEditorManager.Changed += OnChanged;
            Display();
            OOEditorManager.Changed -= OnChanged;


            if (setFocusedControl != null)
            {
                GUI.FocusControl(setFocusedControl);
            }

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
        /// <param name="list">Reference of list to draw.</param>
        public void Draw(List<T> list)
        {
            List = list;

            Draw();
        }

        /// <summary>
        /// If a contained drawer have been modified, the list is processed as changed.
        /// </summary>
        /// <param name="sender">Changed object.</param>
        /// <param name="e">Arguments regarding the change.</param>
        protected void OnChanged(object sender, EventArgs e)
        {
            Changed?.Invoke(this, new ListChangedArgs<T>(List));
        }

        /// <summary>
        /// Displays the list.
        /// </summary>
        protected abstract void Display();
    }
}
