using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Control to edit a line of text.
    /// </summary>
    public class ObjectField<T> : GUIControlField<T>
        where T : Object
    {
        protected override GUIStyle BaseStyle => EditorStyles.textField;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarTextField;
        protected override float ReservedWidth { get; } = 10;
        
        public bool AllowSceneObjects { get; set; }

        /// <summary>
        /// Makes a text field for entering integers.
        /// </summary>
        /// <param name="value">The initial value being edited.</param>
        /// <param name="text">Optional label in front of the field.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the field.</param>
        /// <param name="image">Image to display at the front of the optional label.</param>
        public ObjectField(T value, bool allowSceneObjects, string text = null, string tooltip = null,
            Texture image = null) : base(text, tooltip, image)
        {
            Value = value;
            AllowSceneObjects = allowSceneObjects;
        }

        protected override void Display(Rect position)
        {
            Value = (T)EditorGUI.ObjectField(position, Value, typeof(T), AllowSceneObjects);
        }
    }
}