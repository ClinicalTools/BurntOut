using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class GUISpace : EditorGUIElement
    {
        protected override GUIStyle BaseStyle => EditorStyles.label;

        private float width;
        public override float Width
        {
            get
            {
                return OOEditorManager.ScaledWidth(width, GUIStyle?.fontSize ?? 0);
            }
        }

        public GUISpace(float width) : base(" ")
        {
            this.width = width;
        }

        // Nothing needs to be drawn, as just creating the rectangle reserves the space
        protected override void Display(Rect position) { }
    }
}