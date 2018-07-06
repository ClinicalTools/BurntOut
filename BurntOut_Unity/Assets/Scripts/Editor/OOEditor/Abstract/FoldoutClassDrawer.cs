using System;

namespace OOEditor
{
    /// <summary>
    /// Draws a class in the editor that has a corresponding foldout.
    /// </summary>
    /// <typeparam name="T">The class to draw.</typeparam>
    /// <remarks>Only to be used in lists.</remarks>
    public abstract class FoldoutClassDrawer<T> : ClassDrawer<T>
    {
        /// <summary>
        /// True if the foldout is expanded.
        /// </summary>
        public virtual bool Expanded
        {
            get { return Foldout.Value; }
            set { Foldout.Value = value; }
        }

        private int index;
        /// <summary>
        /// Index of this foldout in the list.
        /// </summary>
        public int Index
        {
            get
            {
                return index;
            }
            set
            {
                if (index != value)
                {
                    index = value;
                    IndexChanged?.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Foldout representing the class drawer.
        /// </summary>
        protected abstract Foldout Foldout { get; }

        /// <summary>
        /// Occurs when the control's value changes.
        /// </summary>
        protected event EventHandler IndexChanged;

        /// <summary>
        /// Initializes a class drawer with a given value.
        /// </summary>
        /// <param name="value">Initial value to set for the class.</param>
        public FoldoutClassDrawer(T value, int index) : base(value)
        {
            Index = index;
        }

        /// <summary>
        /// Draws the foldout representing the class drawer.
        /// </summary>
        public virtual void DrawFoldout()
        {
            Foldout.Draw();
        }
    }
}