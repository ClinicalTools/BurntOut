using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class IntSlider : GUIControlField<int>
    {
        public override int Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        internal override GUIStyle BaseStyle
        {
            get { return EditorStyles.numberField; }
        }

        protected override float AbsoluteMinWidth
        {
            get { return 10; }
        }

        public IntSlider(int value, int min, int max) : base()
        {
            Value = value;
            Min = min;
            Max = max;
        }
        public IntSlider(int value, int min, int max, string text) : base(text)
        {
            Value = value;
            Min = min;
            Max = max;
        }
        public IntSlider(int value, int min, int max, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
            Min = min;
            Max = max;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.IntSlider(position, Value, Min, Max);
        }
    }
}