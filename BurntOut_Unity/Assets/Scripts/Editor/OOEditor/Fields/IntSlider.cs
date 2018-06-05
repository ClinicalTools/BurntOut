using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class IntSlider : GUIControlField<int>
    {
        public override int Value { get; set; }
        public int Min { get; set; }
        public int Max { get; set; }

        protected override GUIStyle BaseStyle
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
            Rect fieldPos = new Rect(position);
            if (fieldPos.width > 50)
            {
                fieldPos.x += fieldPos.width - 50;
                fieldPos.width = 50;
            }

            Value = EditorGUI.IntField(fieldPos, Value, GUIStyle);

            position.width -= 55;
            if (position.width <= 0)
                return;

            if (position.Contains(Event.current.mousePosition))
                GUI.FocusControl(Name);

            EditorGUIUtility.AddCursorRect(position, MouseCursor.SlideArrow);

            if (position.width > 0)
            {
                GUIStyle style = new GUIStyle(GUI.skin.horizontalSliderThumb);
                if (Selected)
                {
                    style.normal = style.focused;
                    style.hover = style.focused;
                    style.active = style.focused;
                }

                Value = Mathf.RoundToInt(GUI.HorizontalSlider(position, Value, Min, Max, GUI.skin.horizontalSlider, style));
            }


            //Value = EditorGUI.IntSlider(position, Value, Min, Max);
        }
    }
}