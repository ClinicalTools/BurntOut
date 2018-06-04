using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class IntSlider : IGuiElement, IControl<int>, IField
    {
        public GUIContent Content { get; set; }
        public int Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }
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

        public IntSlider(int value, int min, int max)
        {
            Value = value;
            Min = min;
            Max = max;
        }
        public IntSlider(int value, int min, int max, string text)
        {
            Value = value;
            Min = min;
            Max = max;
            Content = new GUIContent(text);
        }
        public IntSlider(int value, int min, int max, string text, string tooltip)
        {
            Value = value;
            Min = min;
            Max = max;
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
                Value = EditorGUI.IntSlider(position, Value, Min, Max);
        }
    }
}