using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Popup for selecting a value from an enumerator.
    /// </summary>
    public class EnumPopup : GUIControlField<Enum>
    {
        protected override GUIStyle BaseStyle => EditorStyles.popup;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarPopup;

        /// <summary>
        /// Current width of the popup. 
        /// If FitWidth, width is calculated based on the longest (in pixels) enumerator value.
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
                var names = Enum.GetNames(Value.GetType());
                foreach (var name in names)
                {
                    var content = new GUIContent(name);
                    var width = style.CalcSize(content).x;
                    if (width > longestWidth)
                        longestWidth = width;
                }
                return longestWidth;
            }
        }

        protected override float ReservedWidth { get; } = 20;

        // Default fixedHeight for popups is a fixed number, so that must be fixed
        protected override void ResetGUIStyle()
        {
            base.ResetGUIStyle();

            GUIStyle.fixedHeight = 0;
        }

        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="value">The initial enum value.</param>
        public EnumPopup(Enum value)
        {
            Value = value;
        }
        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="value">The initial enum value.</param>
        /// <param name="text">Optional label in front of the field.</param>
        public EnumPopup(Enum value, string text) : base(text)
        {
            Value = value;
        }
        /// <summary>
        /// Make an enum popup selection field.
        /// </summary>
        /// <param name="value">The initial enum value.</param>
        /// <param name="text">Optional label in front of the popup.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the popup.</param>
        public EnumPopup(Enum value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.EnumPopup(position, Value, GUIStyle);
        }
    }
}