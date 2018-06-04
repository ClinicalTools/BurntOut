using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class EnumPopup : IGuiElement, IControl<Enum>, IField
    {
        public GUIContent Content { get; set; }
        public Enum Value { get; set; }
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
            get { return EditorStyles.popup; }
        }

        public EnumPopup(Enum value)
        {
            Value = value;
        }
        public EnumPopup(Enum value, string text)
        {
            Value = value;
            Content = new GUIContent(text);
        }
        public EnumPopup(Enum value, string text, string tooltip)
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
                Value = EditorGUI.EnumPopup(position, Value);
        }
    }
}