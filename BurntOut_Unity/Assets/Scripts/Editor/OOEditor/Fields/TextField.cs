using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class TextField : IGuiElement, IControl<string>, IField
    {
        public GUIContent Content { get; set; }

        private string value;
        public string Value
        {
            get { return value; }
            set
            {
                if (value == null)
                    this.value = "";
                else
                    this.value = value;
            }
        }
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
            get { return EditorStyles.textField; }
        }

        public TextField(string value)
        {
            Value = value;
        }
        public TextField(string value, string text)
        {
            Value = value;
            Content = new GUIContent(text);
        }
        public TextField(string value, string text, string tooltip)
        {
            Value = value;
            Content = new GUIContent(text, tooltip);
        }

        public void Draw()
        {
            Style.fontSize = OOEditorManager.FontSize;
            OOEditorManager.DrawGuiElement(this, Display, Content);
        }

        private void Display(Rect position)
        {
            if (Content != null)
                position = OOEditorManager.FieldLabel(position, Content, MinWidth);

            if (position.width > 0)
                Value = EditorGUI.TextField(position, Value, Style);
        }
    }
}