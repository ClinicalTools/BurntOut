using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A label to show text.
    /// </summary>
    public class LabelField : EditorGUIElement
    {
        protected override GUIStyle BaseStyle => EditorStyles.label;

        /// <summary>
        /// Makes a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="text">Text to display with the label.</param>
        /// <param name="tooltip">Tooltip of the label.</param>
        /// <param name="image">Image to display at the beginning of the label.</param>
        public LabelField(string text, string tooltip = null, Texture image = null) 
            : base(text, tooltip, image) { }

        protected override void Display(Rect position)
        {
            EditorGUI.LabelField(position, Content, GUIStyle);
        }
    }
}