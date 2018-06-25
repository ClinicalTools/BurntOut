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
        /// Makes a text field for entering integers.
        /// </summary>
        /// <param name="value">The initial value being edited.</param>
        /// <param name="text">Optional label in front of the field.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the field.</param>
        /// <param name="image">Image to display at the front of the optional label.</param>
        public IntField(int value, string text = null, string tooltip = null, Texture image = null)
            : base(text, tooltip, image)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.IntField(position, Value, GUIStyle);
        }
    }
}