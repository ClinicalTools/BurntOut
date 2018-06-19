using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A single press button.
    /// </summary>
    public class Button : GUIControl<bool>
    {
        /// <summary>
        /// Event called when the button is pressed and released.
        /// </summary>
        public event EventHandler Pressed;
        
        protected override GUIStyle BaseStyle => EditorStyles.miniButton;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarButton;
        
        /// <summary>
        /// Makes a single press button.
        /// </summary>
        public Button() { }
        /// <summary>
        /// Makes a single press button.
        /// </summary>
        /// <param name="text">Text to display on the button</param>
        public Button(string text) : base(text) { }
        /// <summary>
        /// Makes a single press button.
        /// </summary>
        /// <param name="text">Text to display on the button</param>
        /// <param name="tooltip">The tooltip of the button</param>
        public Button(string text, string tooltip) : base(text, tooltip) { }

        protected override void Display(Rect position)
        {
            Value = GUI.Button(position, Content, GUIStyle);
            if (Value)
                Pressed?.Invoke(this, new EventArgs());
        }
    }
}