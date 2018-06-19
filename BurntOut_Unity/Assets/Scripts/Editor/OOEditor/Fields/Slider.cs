using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Make a slider the user can drag to change a float value between a min and a max (inclusive).
    /// </summary>
    public class Slider : GUIControlField<float>
    {
        public override float Value
        {
            get { return base.Value; }
            set
            {
                base.Value = Mathf.Clamp(value, Min, Max);
            }
        }
        /// <summary>
        /// The lowest possible value. Located on the left end of the slider.
        /// </summary>
        public float Min { get; set; }
        /// <summary>
        /// The maximum possible value. Located on the right end of the slider.
        /// </summary>
        public float Max { get; set; }

        protected override GUIStyle BaseStyle => EditorStyles.numberField;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarTextField;

        protected override float ReservedWidth { get; } = 10;

        /// <summary>
        /// Make a slider the user can drag to change a float value between a min and a max.
        /// </summary>
        /// <param name="value">The initial value the slider shows. This determines the position of the draggable thumb.</param>
        /// <param name="min">The lowest possible value. Located on the left end of the slider.</param>
        /// <param name="max">The maximum possible value. Located on the right end of the slider.</param>
        public Slider(float value, float min, float max)
        {
            Min = min;
            Max = max;
            Value = value;
        }
        /// <summary>
        /// Make a slider the user can drag to change a float value between a min and a max.
        /// </summary>
        /// <param name="value">The initial value the slider shows. This determines the position of the draggable thumb.</param>
        /// <param name="min">The lowest possible value. Located on the left end of the slider.</param>
        /// <param name="max">The maximum possible value. Located on the right end of the slider.</param>
        /// <param name="text">Optional label in front of the slider.</param>
        public Slider(float value, float min, float max, string text) : base(text)
        {
            Min = min;
            Max = max;
            Value = value;
        }
        /// <summary>
        /// Make a slider the user can drag to change a float value between a min and a max.
        /// </summary>
        /// <param name="value">The initial value the slider shows. This determines the position of the draggable thumb.</param>
        /// <param name="min">The lowest possible value. Located on the left end of the slider.</param>
        /// <param name="max">The maximum possible value. Located on the right end of the slider.</param>
        /// <param name="text">Optional label in front of the slider.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the slider.</param>
        public Slider(float value, float min, float max, string text, string tooltip) : base(text, tooltip)
        {
            Min = min;
            Max = max;
            Value = value;
        }

        // Checks whether the slider portion of the control is selected
        private bool SliderFocused => Focused && !OOEditorManager.FocusedControlName.Contains("field");

        protected override void Display(Rect position)
        {
            // Reserve space for the number portion of the slider
            var fieldPos = new Rect(position);
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
                Value = EditorGUI.FloatField(fieldPos, Value, GUIStyle);
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
                    // Must be slightly greater than .1 to account for rounding errors
                    Value -= .100001f;
                    // Round up to nearest tenth (first press of left will be akin to rounding down)
                    Value = Mathf.Ceil(Value * 10) / 10f;
                    // The control isn't properly updated until a repaint
                    if (EditorWindow.focusedWindow != null)
                        EditorWindow.focusedWindow.Repaint();
                }
                // Pressing right increments the value
                else if (Event.current.rawType == EventType.KeyDown && Event.current.keyCode == KeyCode.RightArrow)
                {
                    Value += .100001f;
                    // Round down to nearest tenth (first press of right will be akin to rounding up)
                    Value = Mathf.Floor(Value * 10) / 10f;
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

                float newValue = GUI.HorizontalSlider(position, Value, Min, Max, GUI.skin.horizontalSlider, style);
                if (Value != newValue)
                    Value = Mathf.Round(newValue * 100) / 100;
            }
            
            // Create the number field representing the same value
            GUI.SetNextControlName(Name + "field");
            Value = EditorGUI.FloatField(fieldPos, Value, GUIStyle);
        }
    }
}