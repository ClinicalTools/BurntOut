using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Toggle : GUIControlField<bool>
    {
        public override bool Value { get; set; }

        internal override GUIStyle BaseStyle
        {
            get { return EditorStyles.toggle; }
        }

        protected override float AbsoluteMinWidth
        {
            get { return 10; }
        }

        public Toggle() : base() { }
        public Toggle(string text) : base(text) { }
        public Toggle(string text, string tooltip) : base(text, tooltip) { }
        public Toggle(bool value) : base()
        {
            Value = value;
        }
        public Toggle(bool value, string text) : base(text)
        {
            Value = value;
        }
        public Toggle(bool value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.Toggle(position, Value, BaseStyle);
        }
    }
}