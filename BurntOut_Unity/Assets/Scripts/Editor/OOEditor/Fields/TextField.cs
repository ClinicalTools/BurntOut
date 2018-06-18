using OOEditor.Internal;
using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class TextField : GUIControlField<string>
    {
        public override string Value
        {
            get { return base.Value; }
            set { base.Value = value ?? ""; }
        }

        protected override GUIStyle BaseStyle => EditorStyles.textField;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarTextField;
        protected override float ReservedWidth { get; } = 10;

        public TextField(string value) : base()
        {
            Value = value;
        }
        public TextField(string value, string text) : base(text)
        {
            Value = value;
        }
        public TextField(string value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.TextField(position, Value, GUIStyle);
        }
    }
}