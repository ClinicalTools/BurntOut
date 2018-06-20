using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Various colors designed to work with Unity's light and dark editor skins.
    /// </summary>
    public static class EditorColors
    {
        // Typically used to highlight errors
        public static Color Error { get; } = new Color(1f, .3f, .3f);

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
                        return new Color(.55f, .9f, .55f);
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
                        return new Color(.8f, 1, .6f);
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
                        return new Color(.95f, .9f, .5f);
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
                        return new Color(1, .6f, .6f);
                    default:
                        return new Color(1f, .7f, .7f);
                }
            }
        }
    }
}