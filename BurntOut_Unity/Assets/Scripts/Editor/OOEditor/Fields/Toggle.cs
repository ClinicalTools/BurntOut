using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Toggle : GUIControlField<bool>
    {
        public override bool Value { get; set; }

        protected override GUIStyle BaseStyle
        {
            get { return EditorStyles.toggle; }
        }

        protected override float AbsoluteMinWidth
        {
            get { return 14; }
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
            Value = EditorGUI.Toggle(position, Value, GUIStyle);

            // For some reason the toggle doesn't always show as active when the style dictates drawing as though it were
            // This grabs the control from the label, if that was clicked, and makes the toggle the focus
            if (Focused && FocusedControlName != Name)
                GUI.FocusControl(Name);
        }
    }
}