using UnityEngine;

namespace OOEditor
{
    public class EditorStyle
    {
        public int FontSize { get; set; }
        public FontStyle? FontStyle { get; set; }

        public void ApplyToStyle(GUIStyle style)
        {
            if (FontSize > 0)
                style.fontSize = FontSize;
            if (FontStyle != null)
                style.fontStyle = (FontStyle)FontStyle;
        }
    }
}