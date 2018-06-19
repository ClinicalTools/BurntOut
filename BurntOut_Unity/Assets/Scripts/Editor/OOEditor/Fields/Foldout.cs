using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A label with a foldout arrow to the left of it.
    /// </summary>
    public class Foldout : GUIControl<bool>
    {
        protected override GUIStyle BaseStyle => EditorStyles.foldout;

        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        public Foldout() { }
        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="text">The label to show.</param>
        public Foldout(string text) : base(text) { }
        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">The tooltip of the foldout.</param>
        public Foldout(string text, string tooltip) : base(text, tooltip) { }
        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="value">Whether the foldout starts expanded.</param>
        public Foldout(bool value)
        {
            Value = value;
        }
        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="value">Whether the foldout starts expanded.</param>
        /// <param name="text">The label to show.</param>
        public Foldout(bool value, string text) : base(text)
        {
            Value = value;
        }
        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="value">Whether the foldout starts expanded.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">The tooltip of the foldout.</param>
        public Foldout(bool value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            //GUIStyle.normal.textColor == new Color();
            Value = EditorGUI.Foldout(position, Value, Content, GUIStyle);
        }
    }
}