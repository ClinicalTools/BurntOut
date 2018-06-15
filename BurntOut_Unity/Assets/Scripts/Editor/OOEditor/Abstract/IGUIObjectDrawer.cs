namespace OOEditor
{
    public interface IGUIObjectDrawer<T>
    {
        T Value { get; set; }
        void Draw();
    }
}