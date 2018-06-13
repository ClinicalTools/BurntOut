using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class IntSlider : GUIControlField<int>
    {
        int value;
        public override int Value
        {
            get { return value; }
            set
            {
                if (value > Max)
                    value = Max;
                else if (value < Min)
                    value = Min;
                this.value = value;
            }
        }
        public int Min { get; set; }
        public int Max { get; set; }

        protected override GUIStyle BaseStyle
        {
            get
            {
                if (OOEditorManager.InToolbar == 0)
                    return EditorStyles.numberField;
                else
                    return EditorStyles.toolbarTextField;
            }
        }

        protected override float AbsoluteMinWidth
        {
            get { return 10; }
        }

        public IntSlider(int value, int min, int max) : base()
        {
            Min = min;
            Max = max;
            Value = value;
        }
        public IntSlider(int value, int min, int max, string text) : base(text)
        {
            Min = min;
            Max = max;
            Value = value;
        }
        public IntSlider(int value, int min, int max, string text, string tooltip) : base(text, tooltip)
        {
            Min = min;
            Max = max;
            Value = value;
        }

        private bool SliderFocused
        {
            get
            {
                return Focused && !FocusedControlName.Contains("field");
            }
        }

        protected override void Display(Rect position)
        {
            Rect fieldPos = new Rect(position);
            if (fieldPos.width > 50)
            {
                fieldPos.x += fieldPos.width - 50;
                fieldPos.width = 50;
            }


            position.width -= 55;

            // If there's not enough width to draw the slider, only draw the textbox
            if (ValidWidth <= 55)
            {
                GUI.SetNextControlName(Name + "field");
                Value = EditorGUI.IntField(fieldPos, Value, GUIStyle);
            }

            if (position.width <= 0)
                return;

            // When mouse is initially clicked on the slider, make the textbox show as selected
            if (position.Contains(Event.current.mousePosition) && Event.current.rawType == EventType.MouseDown)
            {
                GUI.FocusControl(Name);
            }
            else if (SliderFocused)
            {
                // Pressing left decrements the value
                if (Event.current.rawType == EventType.KeyDown && Event.current.keyCode == KeyCode.LeftArrow)
                {
                    Value--;
                    // The control isn't properly updated until a repaint
                    if (EditorWindow.focusedWindow != null)
                        EditorWindow.focusedWindow.Repaint();
                }
                // Pressing right increments the value
                else if (Event.current.rawType == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow)
                {
                    Value++;
                    if (EditorWindow.focusedWindow != null)
                        EditorWindow.focusedWindow.Repaint();
                }
            }

            // The slider should make the mouse act like it should over a slider
            EditorGUIUtility.AddCursorRect(position, MouseCursor.SlideArrow);

            if (position.width > 0)
            {
                GUIStyle style = new GUIStyle(GUI.skin.horizontalSliderThumb);
                // If the slider is focused, paint it like so
                if (SliderFocused)
                    style.normal = style.focused;

                Value = Mathf.RoundToInt(GUI.HorizontalSlider(position, Value, Min, Max, GUI.skin.horizontalSlider, style));
            }

            GUI.SetNextControlName(Name + "field");
            Value = EditorGUI.IntField(fieldPos, Value, GUIStyle);
        }
    }
}