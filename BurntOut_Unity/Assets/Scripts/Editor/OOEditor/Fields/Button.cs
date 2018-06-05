using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Button : GUIControl<bool>
    {
        public override bool Value { get; set; }

        internal override GUIStyle BaseStyle
        {
            get { return EditorStyles.miniButton; }
        }

        public Button() : base() { }
        public Button(string text) : base(text) { }
        public Button(string text, string tooltip) : base(text, tooltip) { }

        protected override void Display(Rect position)
        {
            Value = GUI.Button(position, Content, BaseStyle);
        }
    }
}