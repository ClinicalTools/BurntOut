using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Foldout : GUIControl<bool>
    {
        protected override GUIStyle BaseStyle => EditorStyles.foldout;

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
            //GUIStyle.normal.textColor == new Color();
            Value = EditorGUI.Foldout(position, Value, Content, GUIStyle);
        }
    }
}