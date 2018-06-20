using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Various colors designed to work with Unity's light and dark editor skins.
    /// </summary>
    public static class EditorColors
    {
        // Typically used to highlight errors
        public static Color Error { get; } = new Color(1f, .4f, .4f);

        // Light colors are typically used for a dark theme
        public static Color Green
        {
            get
            {
                switch (GUI.skin.name)
                {
                    case "LightSkin":
                        return new Color(.1f, .5f, .15f);
                    case "DarkSkin":
                        return new Color(.7f, 1f, .8f);
                    default:
                        return new Color(.7f, 1f, .8f);
                }
            }
        }
        public static Color YellowGreen
        {
            get
            {
                switch (GUI.skin.name)
                {
                    case "LightSkin":
                        return new Color(.35f, .45f, .1f);
                    case "DarkSkin":
                        return new Color(.85f, .95f, .7f);
                    default:
                        return new Color(.85f, .95f, .7f);
                }
            }
        }
        public static Color Yellow
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

        public static Color Red
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