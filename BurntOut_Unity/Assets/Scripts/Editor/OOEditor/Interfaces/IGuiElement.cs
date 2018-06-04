using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// An element displayed on the editor GUI.
    /// </summary>
    internal interface IGuiElement
    {
        float MinWidth { get; set; }
        float Width { get; set; }
        float MaxWidth { get; set; }
        GUIStyle Style { get; }

        /// <summary>
        /// Draws the control at the current position.
        /// 
        /// <para>Note that the value may be refreshed with a delay</para>
        /// </summary>
        void Draw();
    }
}