using OOEditor.Internal;
using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class TextField : GUIControlField<string>
    {
        private string value = "";
        public override string Value
        {
            get { return value; }
            set
            {
                if (value == null)
                    this.value = "";
                else
                    this.value = value;
            }
        }

        protected override float AbsoluteMinWidth
        {
            get { return 10; }
        }

        protected override GUIStyle BaseStyle
        {
            get
            {
                if (OOEditorManager.InToolbar == 0)
                    return EditorStyles.textField;
                else
                    return EditorStyles.toolbarTextField;
            }
        }

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