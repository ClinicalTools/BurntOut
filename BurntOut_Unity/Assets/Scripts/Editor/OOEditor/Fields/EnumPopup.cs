using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class EnumPopup : GUIControlField<Enum>
    {
        public override Enum Value { get; set; }

        internal override GUIStyle BaseStyle
        {
            get { return EditorStyles.popup; }
        }

        protected override float AbsoluteMinWidth
        {
            get { return 20; }
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
            Value = EditorGUI.EnumPopup(position, Value, BaseStyle);
        }
    }
}