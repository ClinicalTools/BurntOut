using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class EnumPopup : GUIControlField<Enum>
    {
        public override Enum Value { get; set; }

        protected override GUIStyle BaseStyle
        {
            get
            {
                if (OOEditorManager.InToolbar == 0)
                    return EditorStyles.popup;
                else
                    return EditorStyles.toolbarPopup;
            }
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
            Value = EditorGUI.EnumPopup(position, Value, GUIStyle);
        }
    }
}