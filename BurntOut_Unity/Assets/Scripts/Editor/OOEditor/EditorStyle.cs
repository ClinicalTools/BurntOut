using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Stores font size, style, and color.
    /// </summary>
    public class EditorStyle
    {
        /// <summary>
        /// Size of the font.
        /// 
        /// <para>A value of 0 represents the default font size.</para>
        /// </summary>
        public int FontSize { get; set; }
        /// <summary>
        /// Style of the font. Font can be normal, bold, italics, or bold and italics.
        /// 
        /// <para>A null value represents the default font style.</para>
        /// </summary>
        public FontStyle? FontStyle { get; set; }
        /// <summary>
        /// Color of the font. Recommended to use colors from the EditorColors class.
        /// 
        /// <para>A null value represents the default font color.</para>
        /// </summary>
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

        /// <summary>
        /// Applies this style to a passed GUIStyle.
        /// </summary>
        /// <param name="style">GUIStyle to apply this style to.</param>
        public void ApplyToStyle(GUIStyle style)
        {
            if (FontSize > 0)
                style.fontSize = FontSize;
            if (FontStyle != null)
                style.fontStyle = (FontStyle)FontStyle;
            if (FontColor != null)
            {
                // I believe the first 4 correspond to how an element is drawn by default
                style.active.textColor = (Color)FontColor;
                style.focused.textColor = (Color)FontColor;
                style.hover.textColor = (Color)FontColor;
                style.normal.textColor = (Color)FontColor;
                // I believe the last 4 correspond to how elements with boolean values are often drawn 
                //   after being selected
                style.onActive.textColor = (Color)FontColor;
                style.onFocused.textColor = (Color)FontColor;
                style.onHover.textColor = (Color)FontColor;
                style.onNormal.textColor = (Color)FontColor;
            }
        }
    }
}