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
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="text">Text to display with the label</param>
        public LabelField(string text) : base(text) { }
        public LabelField(string text, string tooltip) : base(text, tooltip) { }

        protected override void Display(Rect position)
        {
            EditorGUI.LabelField(position, Content, GUIStyle);
        }
    }
}