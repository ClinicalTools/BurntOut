using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Control to edit a line of text.
    /// </summary>
    public class TextField : GUIControlField<string>
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
        public TextField(string value)
        {
            Value = value;
        }
        /// <summary>
        /// Makes a text field for entering integers.
        /// </summary>
        /// <param name="value">The initial value being edited.</param>
        /// <param name="text">Optional label in front of the field.</param>
        public TextField(string value, string text) : base(text)
        {
            Value = value;
        }
        /// <summary>
        /// Makes a text field for entering integers.
        /// </summary>
        /// <param name="value">The initial value being edited.</param>
        /// <param name="text">Optional label in front of the field.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the field.</param>
        public TextField(string value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.TextField(position, Value, GUIStyle);
        }
    }
}