using OOEditor.Internal;
using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class ToggleButton : GUIControl<bool>
    {
        public event EventHandler Pressed;

        protected override GUIStyle BaseStyle => EditorStyles.miniButton;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarButton;

        public ToggleButton(bool value) : base()
        {
            Value = value;
        }
        public ToggleButton(bool value, string text) : base(text)
        {
            Value = value;
        }
        public ToggleButton(bool value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            bool lastVal = Value;
            Value = GUI.Toggle(position, Value, Content, GUIStyle);
            if (lastVal != Value)
                Pressed?.Invoke(this, new EventArgs());
        }
    }
}