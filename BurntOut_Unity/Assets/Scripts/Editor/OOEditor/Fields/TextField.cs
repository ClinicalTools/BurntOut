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

        internal override GUIStyle BaseStyle
        {
            get { return EditorStyles.textField; }
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
            Value = EditorGUI.TextField(position, Value, BaseStyle);
        }
    }
}