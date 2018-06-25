using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Makes a slider the user can drag to change an int value between a min and a max (inclusive).
    /// </summary>
    public class IntSlider : GUIControlField<int>
    {
        public override int Value
        {
            get { return base.Value; }
            set
            {
                if (value > Max)
                    value = Max;
                else if (value < Min)
                    value = Min;
                base.Value = value;
            }
        }
        /// <summary>
        /// The lowest possible value. Located on the left end of the slider.
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// The maximum possible value. Located on the right end of the slider.
        /// </summary>
        public int Max { get; set; }

        protected override GUIStyle BaseStyle => EditorStyles.numberField;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarTextField;

        protected override float ReservedWidth { get; } = 10;

        /// <summary>
        /// Makes a slider the user can drag to change an int value between a min and a max.
        /// </summary>
        /// <param name="value">
        /// The initial value the slider shows. This determines the position of the draggable thumb.
        /// </param>
        /// <param name="min">The lowest possible value. Located on the left end of the slider.</param>
        /// <param name="max">The maximum possible value. Located on the right end of the slider.</param>
        /// <param name="text">Optional label in front of the slider.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the slider.</param>
        /// <param name="image">Image to display at the front of the optional label.</param>
        public IntSlider(int value, int min, int max, string text = null, string tooltip = null, 
            Texture image = null) : base(text, tooltip, image)
        {
            Min = min;
            Max = max;
            Value = value;
        }

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

            // Create the number field representing the same value
            GUI.SetNextControlName(Name + "field");
            Value = EditorGUI.IntField(fieldPos, Value, GUIStyle);
        }
    }
}