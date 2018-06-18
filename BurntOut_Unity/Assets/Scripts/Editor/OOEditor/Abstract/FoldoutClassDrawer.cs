namespace OOEditor
{
    /// <summary>
    /// Draws a class in the editor that has a corresponding foldout.
    /// </summary>
    /// <typeparam name="T">The class to draw</typeparam>
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

        /// <summary>
        /// Foldout representing the class drawer.
        /// </summary>
        protected abstract Foldout Foldout { get; }

        /// <summary>
        /// Initializes a class drawer with a given value.
        /// </summary>
        /// <param name="value">Initial value to set for the class.</param>
        public FoldoutClassDrawer(T value) : base(value) { }

        /// <summary>
        /// Draws the foldout representing the class drawer.
        /// </summary>
        public virtual void DrawFoldout()
        {
            Foldout.Draw();
        }
    }
}