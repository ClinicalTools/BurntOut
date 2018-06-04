using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class IntField : IGuiElement, IControl<int>, IField
    {
        public GUIContent Content { get; set; }
        public int Value { get; set; }
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
            get { return EditorStyles.numberField; }
        }

        public IntField(int value)
        {
            Value = value;
        }
        public IntField(int value, string text)
        {
            Value = value;
            Content = new GUIContent(text);
        }
        public IntField(int value, string text, string tooltip)
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
                Value = EditorGUI.IntField(position, Value, Style);
        }
    }
}