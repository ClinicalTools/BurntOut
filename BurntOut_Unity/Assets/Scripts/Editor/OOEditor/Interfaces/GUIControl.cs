
namespace OOEditor
{
    public abstract class GUIControl<T> : GUIElement
    {
        public abstract T Value { get; set; }

        protected GUIControl() : base() { }
        protected GUIControl(string text) : base(text) { }
        protected GUIControl(string text, string tooltip) : base(text, tooltip) { }
    }
}