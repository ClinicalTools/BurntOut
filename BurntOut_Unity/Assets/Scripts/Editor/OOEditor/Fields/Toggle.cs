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

        public override float Width
        {
            get
            {
                if (string.IsNullOrEmpty(Content.text))
                    return ReservedWidth;
                else
                    return 0;
            }
        }
        /// <summary>
        /// Makes a toggle that starts unselected.
        /// </summary>
        /// <param name="text">Optional label in front of the control.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the control.</param>
        /// <param name="image">Image to display at the front of the optional label.</param>
        public Toggle(string text = null, string tooltip = null, Texture image = null)
            : base(text, tooltip, image) { }
        /// <summary>
        /// Makes a toggle.
        /// </summary>
        /// <param name="value">Whether the toggle starts selected.</param>
        /// <param name="text">Optional label in front of the control.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the control.</param>
        /// <param name="image">Image to display at the front of the optional label.</param>
        public Toggle(bool value, string text = null, string tooltip = null, Texture image = null)
            : base(text, tooltip, image)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            position.y += Mathf.Max(0, (position.height / 2) - 9);

            Value = EditorGUI.Toggle(position, Value, GUIStyle);

            // For some reason the toggle doesn't always show as active when the style dictates drawing as though it were
            // This grabs the control from the label, if that was clicked, and makes the toggle the focus
            if (Focused && OOEditorManager.FocusedControlName != Name)
                GUI.FocusControl(Name);
        }
    }
}