using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Reserves empty horizontal space.
    /// </summary>
    public class GUISpace : EditorGUIElement
    {
        protected override GUIStyle BaseStyle => EditorStyles.label;

        private float width;
        /// <summary>
        /// Gets the scaled width based on the current font-size.
        /// </summary>
        public override float Width => OOEditorManager.ScaledWidth(width, GUIStyle?.fontSize ?? 0);

        /// <summary>
        /// Makes an element to reserve horizontal space.
        /// </summary>
        /// <param name="width">Width to reserve at a font-size of 11.</param>
        public GUISpace(float width) : base(" ")
        {
            this.width = width;
        }

        // Nothing needs to be drawn, as just creating the rectangle reserves the space
        protected override void Display(Rect position) { }
    }
}