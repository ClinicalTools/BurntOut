using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Foldout : GUIControl<bool>
    {
        public override bool Value { get; set; }

        internal override GUIStyle BaseStyle
        {
            get { return EditorStyles.foldout; }
        }

        public Foldout() : base() { }
        public Foldout(string text) : base(text) { }
        public Foldout(string text, string tooltip) : base(text, tooltip) { }
        public Foldout(bool value) : base()
        {
            Value = value;
        }
        public Foldout(bool value, string text) : base(text)
        {
            Value = value;
        }
        public Foldout(bool value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.Foldout(position, Value, Content, BaseStyle);
        }
    }
}