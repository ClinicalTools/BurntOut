namespace OOEditor
{
    public interface IGUIObjectDrawer<T>
    {
        T Value { get; set; }
        /// <summary>
        /// Updates the object's value and then draws it.
        /// </summary>
        /// <param name="value">Updated value to draw</param>
        void Draw(T value);
    }
}