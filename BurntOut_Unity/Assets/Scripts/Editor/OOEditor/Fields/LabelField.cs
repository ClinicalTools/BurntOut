using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A label to show text.
    /// </summary>
    public class LabelField : EditorGUIElement
    {
        protected override GUIStyle BaseStyle => EditorStyles.label;

        /// <summary>
        /// Makes a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="text">Text to display with the label.</param>
        /// <param name="tooltip">Tooltip of the label.</param>
        /// <param name="image">Image to display at the beginning of the label.</param>
        public LabelField(string text, string tooltip = null, Texture image = null)
            : base(text, tooltip, image) { }

        /// <summary>
        /// Makes a label field to label another editor element. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="connectedElement">Element to connect this label with.</param>
        /// <param name="text">Text to display with the label.</param>
        /// <param name="tooltip">Tooltip of the label.</param>
        /// <param name="image">Image to display at the beginning of the label.</param>
        public LabelField(EditorGUIElement connectedElement, string text, string tooltip = null, Texture image = null)
            : base(text, tooltip, image)
        {
            ConnectToElement(connectedElement);
        }

        private EditorGUIElement connectedElement;

        /// <summary>
        /// Connects the label to another GUI element.
        /// </summary>
        /// <param name="connectedElement">Element to connect this label with.</param>
        public void ConnectToElement(EditorGUIElement connectedElement)
        {
            this.connectedElement = connectedElement;

            if (connectedElement?.Name == null)
                Name = (ElementNums++).ToString("X8");
            else
                Name = connectedElement.Name;
        }

        protected override void Display(Rect position)
        {
            // If a label linked to an element is clicked, select it
            if (connectedElement != null)
            {
                if (Event.current.rawType == EventType.MouseDown &&
                    position.Contains(Event.current.mousePosition))
                {
                    GUI.FocusControl(connectedElement.Name);

                    // Needed for things to be selected correctly
                    EditorWindow.focusedWindow.Repaint();
                }
            }

            EditorGUI.LabelField(position, Content, GUIStyle);
        }
    }
}