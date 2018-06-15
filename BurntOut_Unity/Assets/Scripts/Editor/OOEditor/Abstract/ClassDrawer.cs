using OOEditor;

public abstract class ClassDrawer<T> : IGUIObjectDrawer<T>
{
    public virtual T Value { get; set; }

    public ClassDrawer(T value)
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

    protected abstract void Display();
}
