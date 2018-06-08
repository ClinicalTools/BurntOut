using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class ToggleButton : GUIControl<bool>
    {
        public override bool Value { get; set; }

        protected override GUIStyle BaseStyle
        {
            get
            {
                if (OOEditorManager.InToolbar == 0)
                    return EditorStyles.miniButton;
                else
                    return EditorStyles.toolbarButton;
            }
        }

        public ToggleButton(bool value) : base()
        {
            Value = value;
        }
        public ToggleButton(bool value, string text) : base(text)
        {
            Value = value;
        }
        public ToggleButton(bool value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = GUI.Toggle(position, Value, Content, GUIStyle);
        }
    }
}