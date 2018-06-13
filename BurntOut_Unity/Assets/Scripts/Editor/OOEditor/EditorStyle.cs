using UnityEngine;

namespace OOEditor
{
    public class EditorStyle
    {
        public int FontSize { get; set; }
        public FontStyle? FontStyle { get; set; }
        public Color? FontColor { get; set; }

        public EditorStyle() { }
        public EditorStyle(Color fontColor)
        {
            FontColor = fontColor;
        }
        public EditorStyle(FontStyle fontStyle)
        {
            FontStyle = fontStyle;
        }
        public EditorStyle(FontStyle fontStyle, Color fontColor)
        {
            FontStyle = fontStyle;
            FontColor = fontColor;
        }
        public EditorStyle(int fontSize)
        {
            FontSize = fontSize;
        }
        public EditorStyle(int fontSize, FontStyle fontStyle)
        {
            FontSize = fontSize;
            FontStyle = fontStyle;
        }
        public EditorStyle(int fontSize, FontStyle fontStyle, Color fontColor)
        {
            FontSize = fontSize;
            FontStyle = fontStyle;
            FontColor = fontColor;
        }

        public void ApplyToStyle(GUIStyle style)
        {
            if (FontSize > 0)
                style.fontSize = FontSize;
            if (FontStyle != null)
                style.fontStyle = (FontStyle)FontStyle;
            if (FontColor != null)
            {
                style.active.textColor = (Color)FontColor;
                style.focused.textColor = (Color)FontColor;
                style.hover.textColor = (Color)FontColor;
                style.normal.textColor = (Color)FontColor;

                style.onActive.textColor = (Color)FontColor;
                style.onFocused.textColor = (Color)FontColor;
                style.onHover.textColor = (Color)FontColor;
                style.onNormal.textColor = (Color)FontColor;
            }
        }
    }
}