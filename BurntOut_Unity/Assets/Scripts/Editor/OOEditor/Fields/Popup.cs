using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Popup for selecting a value from an enumerator.
    /// </summary>
    public class Popup : GUIControlField<int>
    {
        protected override GUIStyle BaseStyle => EditorStyles.popup;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarPopup;

        /// <summary>
        /// Current width of the popup. 
        /// If FitWidth, width is calculated based on the longest (in pixels) possible value.
        /// </summary>
        public override float Width
        {
            get
            {
                // Only elements without labels that fit their width need a width calculated
                if (!FitWidth || Content != null)
                    return 0;

                var style = GUIStyle;

                var longestWidth = 0f;
                foreach (var option in Options)
                {
                    var content = new GUIContent(option);
                    var width = style.CalcSize(content).x;
                    if (width > longestWidth)
                        longestWidth = width;
                }
                return longestWidth;
            }
        }

        protected override float ReservedWidth { get; } = 20;

        protected override void ResetGUIStyle()
        {
            base.ResetGUIStyle();

            GUIStyle.fixedHeight = 0;
        }

        /// <summary>
        /// Options the user has to select from.
        /// </summary>
        public string[] Options { get; set; }


        /// <summary>
        /// Makes a popup selection field.
        /// </summary>
        /// <param name="value">The initial enum value.</param>
        /// <param name="options">Options the user has to select from.</param>
        public Popup(int value, string[] options) 
        {
            Options = options;
            Value = value;
        }
        /// <summary>
        /// Makes a popup selection field.
        /// </summary>
        /// <param name="value">The initial enum value.</param>
        /// <param name="options">Options the user has to select from.</param>
        /// <param name="text">Optional label in front of the popup.</param>
        public Popup(int value, string[] options, string text) : base(text)
        {
            Options = options;
            Value = value;
        }
        /// <summary>
        /// Makes a popup selection field.
        /// </summary>
        /// <param name="value">The initial enum value.</param>
        /// <param name="options">Options the user has to select from.</param>
        /// <param name="text">Optional label in front of the popup.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the popup.</param>
        public Popup(int value, string[] options, string text, string tooltip) : base(text, tooltip)
        {
            Options = options;
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.Popup(position, Value, Options, GUIStyle);
        }
    }
}