using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A button that toggles being selected when clicked.
    /// </summary>
    public class ToggleButton : GUIControl<bool>
    {
        public event EventHandler Pressed;

        protected override GUIStyle BaseStyle => EditorStyles.miniButton;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarButton;

        /// <summary>
        /// Makes a button that's toggled on click.
        /// </summary>
        /// <param name="value">True if the button starts toggled.</param>
        public ToggleButton(bool value)
        {
            Value = value;
        }
        /// <summary>
        /// Makes a button that's toggled on click.
        /// </summary>
        /// <param name="value">True if the button starts toggled.</param>
        /// <param name="text">Text to display on the toggle button.</param>
        public ToggleButton(bool value, string text) : base(text)
        {
            Value = value;
        }
        /// <summary>
        /// Makes a button that's toggled on click.
        /// </summary>
        /// <param name="value">True if the button starts toggled.</param>
        /// <param name="text">Text to display on the toggle button.</param>
        /// <param name="tooltip">The tooltip of the toggle button.</param>
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