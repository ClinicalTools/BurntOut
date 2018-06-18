using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class EnumPopup : GUIControlField<Enum>
    {
        protected override GUIStyle BaseStyle => EditorStyles.popup;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarPopup;

        public override float Width
        {
            get
            {
                if (FitWidth && Content == null)
                {
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

                return 0;
            }
        }

        protected override float ReservedWidth { get; } = 20;

        protected override void ResetGUIStyle()
        {
            base.ResetGUIStyle();

            GUIStyle.fixedHeight = 0;
        }

        public EnumPopup(Enum value) : base()
        {
            Value = value;
        }
        public EnumPopup(Enum value, string text) : base(text)
        {
            Value = value;
        }
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