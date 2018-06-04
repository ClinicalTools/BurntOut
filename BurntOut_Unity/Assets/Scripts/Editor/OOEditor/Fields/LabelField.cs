using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class LabelField : IGuiElement, IField
    {
        public GUIContent Content { get; set; }

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
            get { return EditorStyles.label; }
        }

        public LabelField()
        {
            Content = new GUIContent();
        }
        public LabelField(string text)
        {
            Content = new GUIContent(text);
        }
        public LabelField(string text, string tooltip)
        {
            Content = new GUIContent(text, tooltip);
        }

        public void Draw()
        {
            OOEditorManager.DrawGuiElement(this, Display, Content);
        }

        private void Display(Rect position)
        {
            EditorGUI.LabelField(position, Content, Style);
        }
    }
}