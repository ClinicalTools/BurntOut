using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Popup : GUIControlField<int>
    {
        protected override GUIStyle BaseStyle => EditorStyles.popup;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarPopup;

        protected override float AbsoluteMinWidth { get; } = 20;
        public string[] Options { get; set; }

        public Popup(int value, string[] options) : base()
        {
            Options = options;
            Value = value;
        }
        public Popup(int value, string[] options, string text) : base(text)
        {
            Options = options;
            Value = value;
        }
        public Popup(int value, string[] options, string text, string tooltip) : base(text, tooltip)
        {
            Options = options;
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.Popup(position, Value, Options, GUIStyle);
        }
    }
}