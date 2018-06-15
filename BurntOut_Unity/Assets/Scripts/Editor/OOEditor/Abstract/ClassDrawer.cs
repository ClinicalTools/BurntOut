using OOEditor;

public abstract class ClassDrawer<T> : IGUIObjectDrawer<T>
{
    private T value;
    public virtual T Value
    {
        get
        {
            return value;
        }
        set
        {
            if (this.value != null && !this.value.Equals(value))
            {
                this.value = value;
                ResetValues();
            }
            else
            {
                this.value = value;
            }
        }
    }

    public abstract void ResetValues();
    public abstract void Draw();
}
