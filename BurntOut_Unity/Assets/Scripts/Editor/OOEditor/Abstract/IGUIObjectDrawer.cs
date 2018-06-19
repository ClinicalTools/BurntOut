namespace OOEditor
{
    /// <summary>
    /// Basic draw functionality required for GUI objects that store a value.
    /// </summary>
    /// <typeparam name="T">Type of value to store</typeparam>
    public interface IGUIObjectDrawer<T>
    {
        /// <summary>
        /// Value being represented by the drawer.
        /// </summary>
        T Value { get; set; }
        /// <summary>
        /// Updates the object's value and then draws it.
        /// </summary>
        /// <param name="value">Updated value to draw.</param>
        void Draw(T value);
    }
}