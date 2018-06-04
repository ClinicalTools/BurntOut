namespace OOEditor
{
    /// <summary>
    /// Any GUI element that holds a value.
    /// </summary>
    /// <typeparam name="T">Value held by the control</typeparam>
    internal interface IControl<T>
    {
        T Value { get; set; }
    }
}