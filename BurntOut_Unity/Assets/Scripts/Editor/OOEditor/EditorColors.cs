using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Various colors designed to work with Unity's Professional editor skin.
    /// </summary>
    /// <remarks>Later, these will preferably return a color based on the skin</remarks>
    public static class EditorColors
    {
        // Typically used to highlight errors
        public static Color Red { get; } = new Color(1f, .4f, .4f);

        // Light colors are typically used for a dark theme
        public static Color LightGreen
        {
            get
            {
                switch (GUI.skin.name)
                {
                    case "LightSkin":
                        return new Color(.1f, .5f, .1f);
                    case "DarkSkin":
                        return new Color(.7f, 1f, .7f);
                    default:
                        return new Color(.7f, 1f, .7f);
                }
            }
        }
        public static Color LightYellow
        {
            get
            {
                switch (GUI.skin.name)
                {
                    case "LightSkin":
                        return new Color(.5f, .5f, .1f);
                    case "DarkSkin":
                        return new Color(1f, 1f, .7f);
                    default:
                        return new Color(1f, 1f, .7f);
                }
            }
        }

        public static Color LightRed
        {
            get
            {
                switch (GUI.skin.name)
                {
                    case "LightSkin":
                        return new Color(.5f, .1f, .1f);
                    case "DarkSkin":
                        return new Color(1f, .7f, .7f);
                    default:
                        return new Color(1f, .7f, .7f);
                }
            }
        }
    }
}