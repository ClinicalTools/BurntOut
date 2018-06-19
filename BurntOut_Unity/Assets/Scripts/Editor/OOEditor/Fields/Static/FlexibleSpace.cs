using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Creates flexible space to be used with horizontals and toolbars.
    /// </summary>
    public static class FlexibleSpace
    {
        /// <summary>
        /// Draws flexible space.
        /// To be used in a horizontal or toolbar.
        /// </summary>
        public static void Draw()
        {
            GUILayout.FlexibleSpace();
        }
    }
}