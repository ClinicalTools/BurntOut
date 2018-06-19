namespace OOEditor
{
    /// <summary>
    /// Draws a class in the editor.
    /// </summary>
    /// <typeparam name="T">The class to draw</typeparam>
    public abstract class ClassDrawer<T> : IGUIObjectDrawer<T>
    {
        /// <summary>
        /// Value being represented by the drawer.
        /// </summary>
        public virtual T Value { get; set; }

        /// <summary>
        /// Initializes a class drawer with a given value.
        /// </summary>
        /// <param name="value">Initial value to set for the class</param>
        protected ClassDrawer(T value)
        {
            Value = value;
        }

        /// <summary>
        /// Updates the class's value and then draws it.
        /// </summary>
        /// <param name="value">Updated value to draw</param>
        public void Draw(T value)
        {
            Value = value;

            Display();
        }

        /// <summary>
        /// Used to draw the class in the editor.
        /// </summary>
        /// <remarks>Similar to Draw, but not always executed immediately after draw is called.</remarks>
        protected abstract void Display();
    }
}