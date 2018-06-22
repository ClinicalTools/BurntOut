using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Various colors designed to work with Unity's light and dark editor skins.
    /// </summary>
    public static class EditorColors
    {
        // Typically used to highlight errors
        public static Color Error { get; } = new Color(10, 0, 0);

        // Light colors are typically used for a dark theme
        public static Color Green { get; } = new Color(0, 1, 0);
        public static Color YellowGreen { get; } = new Color(.5f, 1, 0);
        public static Color Yellow { get; } = new Color(1, 1, 0);
        public static Color Red { get; } = new Color(1, 0, 0);
    }
}