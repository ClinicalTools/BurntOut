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
                BlendColors(style.normal.textColor, (Color)FontColor);

                // I believe the first 4 correspond to how an element is drawn by default
                style.active.textColor = BlendColors(style.active.textColor, (Color)FontColor);
                style.focused.textColor = BlendColors(style.focused.textColor, (Color)FontColor);
                style.hover.textColor = BlendColors(style.hover.textColor, (Color)FontColor);
                style.normal.textColor = BlendColors(style.normal.textColor, (Color)FontColor);
                // I believe the last 4 correspond to how elements with boolean values are often drawn 
                //   after being selected
                style.onActive.textColor = BlendColors(style.onActive.textColor, (Color)FontColor);
                style.onFocused.textColor = BlendColors(style.onFocused.textColor, (Color)FontColor);
                style.onHover.textColor = BlendColors(style.onHover.textColor, (Color)FontColor);
                style.onNormal.textColor = BlendColors(style.onNormal.textColor, (Color)FontColor);
            }
        }

        // Blends two colors, keeping color magnitude of the original's, or 50%, whichever is more
        // Multiplies the alphas
        private Color BlendColors(Color original, Color blend)
        {
            var c1Height = Mathf.Sqrt(original.r * original.r + original.g * original.g + 
                original.b * original.b);
            // Minimum magnitude to ensure we don't see 
            var colorHeight = Mathf.Max(c1Height, .6f);

            var vec = new Vector3((original.r + blend.r) / 2, (original.g + blend.g) / 2,
                 (original.b + blend.b) / 2);
            vec.Normalize();
            vec *= colorHeight;
            // Cap colors at 1, with the exception of green. Since it's a brighter color, cap it lower
            var newColor = new Color(Mathf.Min(vec.x, 1), Mathf.Min(vec.y, .9f), Mathf.Min(vec.z, 1),
                original.a * blend.a);

            return newColor;
        }
    }
}