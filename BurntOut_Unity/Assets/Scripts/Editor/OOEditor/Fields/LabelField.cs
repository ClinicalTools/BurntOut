using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class LabelField : GUIElement
    {
        internal override GUIStyle BaseStyle
        {
            get { return EditorStyles.label; }
        }

        public LabelField() : base()
        {
            Content = new GUIContent();
        }
        public LabelField(string text) : base(text)
        {
            Content = new GUIContent(text);
        }
        public LabelField(string text, string tooltip) : base(text, tooltip)
        {
            Content = new GUIContent(text, tooltip);
        }

        protected override void Display(Rect position)
        {
            EditorGUI.LabelField(position, Content, BaseStyle);
        }
    }
}