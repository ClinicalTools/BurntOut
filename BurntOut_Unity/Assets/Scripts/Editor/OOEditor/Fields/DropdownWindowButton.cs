using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A button that reacts to mouse down, for displaying your own dropdown content.
    /// </summary>
    public class DropdownMenuButton : GUIControl<bool>
    {
        /// <summary>
        /// Event called when the button is pressed and released.
        /// </summary>
        public event EventHandler Pressed;

        protected override GUIStyle BaseStyle => EditorStyles.miniButton;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarButton;

        public GenericMenu Menu { get; set; }

        /// <summary>
        /// Make a button that reacts to mouse down, for displaying your own dropdown content.
        /// </summary>
        /// <param name="menu">Menu to display on a button press.</param>
        /// <param name="text">Text to display on the button.</param>
        /// <param name="tooltip">The tooltip of the button.</param>
        /// <param name="image">Image to display on the button.</param>
        public DropdownMenuButton(GenericMenu menu, string text = null, string tooltip = null, 
            Texture image = null) : base(text, tooltip, image)
        {
            Menu = menu;
        }

        protected override void Display(Rect position)
        {
            Value = GUI.Button(position, Content, GUIStyle);
            if (Value)
            {
                Menu.DropDown(position);
                Pressed?.Invoke(this, new EventArgs());
            }
        }
    }
}