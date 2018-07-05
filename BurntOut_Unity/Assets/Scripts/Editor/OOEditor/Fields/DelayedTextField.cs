using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Control to edit a line of text.
    /// </summary>
    public class DelayedTextField : GUIControlField<string>
    {
        /// <summary>
        /// Value being represented by the control.
        /// </summary>
        public override string Value
        {
            get { return base.Value; }
            set { base.Value = value ?? ""; }
        }

        protected override GUIStyle BaseStyle => EditorStyles.textField;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarTextField;
        protected override float ReservedWidth { get; } = 10;

        /// <summary>
        /// Makes a text field for entering integers.
        /// </summary>
        /// <param name="value">The initial value being edited.</param>
        /// <param name="text">Optional label in front of the field.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the field.</param>
        /// <param name="image">Image to display at the front of the optional label.</param>
        public DelayedTextField(string value, string text = null, string tooltip = null,
            Texture image = null) : base(text, tooltip, image)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.DelayedTextField(position, Value, GUIStyle);
        }
    }
}