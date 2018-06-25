using OOEditor.Internal;
using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A control that can be toggled.
    /// </summary>
    public class ToggleLeft : GUIControl<bool>
    {
        /// <summary>
        /// Occurs when the control is pressed.
        /// </summary>
        public event EventHandler<ControlChangedArgs<bool>> Pressed;

        // Using the Toggle style draws two toggle bokes
        protected override GUIStyle BaseStyle =>
            new GUIStyle(EditorStyles.label)
            {
                imagePosition = ImagePosition.ImageAbove
            };

        /// <summary>
        /// Makes a toggle that starts unselected.
        /// </summary>
        /// <param name="text">Optional label in front of the control.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the control.</param>
        /// <param name="image">Image to display at the front of the optional label.</param>
        public ToggleLeft(string text = null, string tooltip = null, Texture image = null)
            : base(text, tooltip, image) { }
        /// <summary>
        /// Makes a toggle.
        /// </summary>
        /// <param name="value">Whether the toggle starts selected.</param>
        /// <param name="text">Optional label in front of the control.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the control.</param>
        /// <param name="image">Image to display at the front of the optional label.</param>
        public ToggleLeft(bool value, string text = null, string tooltip = null, Texture image = null)
            : base(text, tooltip, image)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Rect togglePos = new Rect(position.x, position.y + position.height / 2 - 6,
                16, position.height);
            position.x += 16;
            position.width -= 16;

            var lastValue = Value;
            Value = EditorGUI.ToggleLeft(togglePos, "", Value, GUIStyle);
            EditorGUI.LabelField(position, Content, GUIStyle);

            if (Value != lastValue)
                Pressed?.Invoke(this, new ControlChangedArgs<bool>(lastValue, Value));

            // For some reason the toggle doesn't always show as active when the style dictates drawing as though it were
            // This grabs the control from the label, if that was clicked, and makes the toggle the focus
            if (Focused && OOEditorManager.FocusedControlName != Name)
                GUI.FocusControl(Name);
        }
    }
}