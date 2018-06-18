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
                return width;
            }
        }

        public GUISpace(float width) : base(" ")
        {
            this.width = width;
        }

        protected override void Display(Rect position)
        {
            GUILayout.Space(position.x);
        }
    }
}