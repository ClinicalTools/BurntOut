using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Text field for entering integers.
    /// </summary>
    public class IntField : GUIControlField<int>
    {
        protected override GUIStyle BaseStyle => EditorStyles.numberField;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarTextField;
        
        protected override float ReservedWidth { get; } = 10;

        /// <summary>
        /// Make a text field for entering integers.
        /// </summary>
        /// <param name="value">The initial value being edited</param>
        public IntField(int value)
        {
            Value = value;
        }
        /// <summary>
        /// Make a text field for entering integers.
        /// </summary>
        /// <param name="value">The initial value being edited</param>
        /// <param name="text">Optional label in front of the field</param>
        public IntField(int value, string text) : base(text)
        {
            Value = value;
        }
        /// <summary>
        /// Make a text field for entering integers.
        /// </summary>
        /// <param name="value">The initial value being edited</param>
        /// <param name="text">Optional label in front of the field</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the field</param>
        public IntField(int value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.IntField(position, Value, GUIStyle);
        }
    }
}