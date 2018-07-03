using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A label with a foldout arrow to the left of it.
    /// </summary>
    public class Foldout : GUIControl<bool>
    {
        protected override GUIStyle BaseStyle =>
            new GUIStyle(EditorStyles.foldout)
            {
                imagePosition = ImagePosition.ImageAbove
            };

        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">The tooltip of the foldout.</param>
        /// <param name="image">Image to draw at the beginning of the foldout.</param>
        public Foldout(string text = null, string tooltip = null, Texture image = null) 
            : base(text, tooltip, image) { }
        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="value">Whether the foldout starts expanded.</param>
        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="value">Whether the foldout starts expanded.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">The tooltip of the foldout.</param>
        /// <param name="image">Image to draw at the beginning of the foldout.</param>
        public Foldout(bool value, string text = null, string tooltip = null, Texture image = null)
            : base(text, tooltip, image)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.Foldout(position, Value, Content, GUIStyle);
        }
    }
}