using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Button : GUIControl<bool>
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

        public Button() : base() { }
        public Button(string text) : base(text) { }
        public Button(string text, string tooltip) : base(text, tooltip) { }

        protected override void Display(Rect position)
        {
            Value = GUI.Button(position, Content, GUIStyle);
        }
    }
}