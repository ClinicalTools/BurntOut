using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class IntField : GUIControlField<int>
    {
        public override int Value { get; set; }

        internal override GUIStyle BaseStyle
        {
            get { return EditorStyles.numberField; }
        }
        protected override float AbsoluteMinWidth
        {
            get { return 10; }
        }

        public IntField(int value) : base()
        {
            Value = value;
        }
        public IntField(int value, string text) : base(text)
        {
            Value = value;
        }
        public IntField(int value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.IntField(position, Value, BaseStyle);
        }
    }
}