using UnityEngine;

namespace OOEditor
{
    public class EditorStyle
    {
        public int FontSize { get; set; }
        public FontStyle? FontStyle { get; set; }

        public EditorStyle() { }
        public EditorStyle(int fontSize)
        {
            FontSize = fontSize;
        }
        public EditorStyle(FontStyle fontStyle)
        {
            FontStyle = fontStyle;
        }
        public EditorStyle(int fontSize, FontStyle fontStyle)
        {
            FontSize = fontSize;
            FontStyle = fontStyle;
        }

        public void ApplyToStyle(GUIStyle style)
        {
            if (FontSize > 0)
                style.fontSize = FontSize;
            if (FontStyle != null)
                style.fontStyle = (FontStyle)FontStyle;
        }
    }
}