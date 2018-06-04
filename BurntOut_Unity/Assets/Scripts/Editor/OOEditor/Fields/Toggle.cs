using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Toggle : IGuiElement, IControl<bool>, IField
    {
        public GUIContent Content { get; set; }
        public bool Value { get; set; }
        private float minWidth = 0;
        public float MinWidth
        {
            get
            {
                return minWidth;
            }
            set
            {
                minWidth = value;
            }
        }
        public float Width { get; set; }
        public float MaxWidth { get; set; }

        public GUIStyle Style
        {
            get { return EditorStyles.toggle; }
        }

        public Toggle()
        {
            Content = new GUIContent("");
        }
        public Toggle(string text)
        {
            Content = new GUIContent(text);
        }
        public Toggle(string text, string tooltip)
        {
            Content = new GUIContent(text, tooltip);
        }

        public void Draw()
        {
            Style.fontSize = OOEditorManager.FontSize;
            OOEditorManager.DrawGuiElement(this, Display, Content);
        }

        private void Display(Rect position)
        {
            Value = EditorGUI.Toggle(position, Content, Value, Style);
        }
    }
}