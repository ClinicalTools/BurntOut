using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Foldout : IGuiElement, IControl<bool>
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
            get { return EditorStyles.foldout; }
        }

        public Foldout(string text)
        {
            Content = new GUIContent(text);
        }
        public Foldout(string text, string tooltip)
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
            Value = EditorGUI.Foldout(position, Value, Content, Style);
        }
    }
}