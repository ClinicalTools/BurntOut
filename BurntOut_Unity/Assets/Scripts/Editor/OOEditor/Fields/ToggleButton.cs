using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class ToggleButton : IGuiElement, IControl<bool>
    {
        public GUIContent Content { get; set; }
        public bool Value { get; set; }
        private float minWidth = 20;
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
            get { return EditorStyles.miniButton; }
        }

        public ToggleButton()
        {
            Content = new GUIContent("");
        }
        public ToggleButton(string text)
        {
            Content = new GUIContent(text);
        }
        public ToggleButton(string text, string tooltip)
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
            Value = GUI.Toggle(position, Value, Content, Style);
        }
    }
}