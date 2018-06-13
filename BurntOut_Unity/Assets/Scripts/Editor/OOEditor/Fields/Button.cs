using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class Button : GUIControl<bool>
    {
        public event EventHandler Pressed;
        
        protected override GUIStyle BaseStyle => EditorStyles.miniButton;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarButton;

        public Button() : base() { }
        public Button(string text) : base(text) { }
        public Button(string text, string tooltip) : base(text, tooltip) { }

        protected override void Display(Rect position)
        {
            Value = GUI.Button(position, Content, GUIStyle);
            if (Value && Pressed != null)
                Pressed(this, null);
        }
    }
}