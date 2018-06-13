using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class EnumPopup : GUIControlField<Enum>
    {
        protected override GUIStyle BaseStyle => EditorStyles.popup;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarPopup;

        protected override float AbsoluteMinWidth { get; } = 20;
        
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