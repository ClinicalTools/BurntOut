using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Popup : GUIControlField<int>
    {
        protected override GUIStyle BaseStyle => EditorStyles.popup;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarPopup;

        public override float Width
        {
            get
            {
                if (FitWidth && Content == null)
                {
                    var style = GUIStyle;

                    var longestWidth = 0f;
                    foreach (var option in Options)
                    {
                        var content = new GUIContent(option);
                        var width = style.CalcSize(content).x;
                        if (width > longestWidth)
                            longestWidth = width;
                    }
                    return longestWidth;
                }

                return 0;
            }
        }

        protected override float AbsoluteMinWidth { get; } = 20;

        protected override void ResetGUIStyle()
        {
            base.ResetGUIStyle();

            GUIStyle.fixedHeight = 0;
        }

        public string[] Options { get; set; }

        public Popup(int value, string[] options) : base()
        {
            Options = options;
            Value = value;
        }
        public Popup(int value, string[] options, string text) : base(text)
        {
            Options = options;
            Value = value;
        }
        public Popup(int value, string[] options, string text, string tooltip) : base(text, tooltip)
        {
            Options = options;
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.Popup(position, Value, Options, GUIStyle);
        }
    }
}