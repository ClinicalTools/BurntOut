using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class FloatSlider : GUIControlField<float>
    {
        public override float Value { get; set; }
        public float Min { get; set; }
        public float Max { get; set; }

        protected override GUIStyle BaseStyle
        {
            get { return EditorStyles.numberField; }
        }

        protected override float AbsoluteMinWidth
        {
            get { return 10; }
        }

        public FloatSlider(float value, float min, float max) : base()
        {
            Value = value;
            Min = min;
            Max = max;
        }
        public FloatSlider(float value, float min, float max, string text) : base(text)
        {
            Value = value;
            Min = min;
            Max = max;
        }
        public FloatSlider(float value, float min, float max, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
            Min = min;
            Max = max;
        }

        protected override void Display(Rect position)
        {
            Value = GUI.HorizontalSlider(position, Value, Min, Max);
        }
    }
}