using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A control that can be toggled.
    /// </summary>
    public class Toggle : GUIControlField<bool>
    {
        protected override GUIStyle BaseStyle => EditorStyles.toggle;

        protected override float ReservedWidth { get; } = 14;

        /// <summary>
        /// Makes a toggle that starts unselected.
        /// </summary>
        public Toggle() { }
        /// <summary>
        /// Makes a toggle that starts unselected.
        /// </summary>
        /// <param name="text">Optional label in front of the control.</param>
        public Toggle(string text) : base(text) { }
        /// <summary>
        /// Makes a toggle that starts unselected.
        /// </summary>
        /// <param name="text">Optional label in front of the control.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the control.</param>
        public Toggle(string text, string tooltip) : base(text, tooltip) { }
        /// <summary>
        /// Makes a toggle.
        /// </summary>
        /// <param name="value">Whether the toggle starts selected.</param>
        public Toggle(bool value)
        {
            Value = value;
        }
        /// <summary>
        /// Makes a toggle.
        /// </summary>
        /// <param name="value">Whether the toggle starts selected.</param>
        /// <param name="text">Optional label in front of the control.</param>
        public Toggle(bool value, string text) : base(text)
        {
            Value = value;
        }
        /// <summary>
        /// Makes a toggle.
        /// </summary>
        /// <param name="value">Whether the toggle starts selected.</param>
        /// <param name="text">Optional label in front of the control.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the control.</param>
        public Toggle(bool value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.Toggle(position, Value, GUIStyle);

            // For some reason the toggle doesn't always show as active when the style dictates drawing as though it were
            // This grabs the control from the label, if that was clicked, and makes the toggle the focus
            if (Focused && OOEditorManager.FocusedControlName != Name)
                GUI.FocusControl(Name);
        }
    }
}