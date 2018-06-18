using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class LabelField : EditorGUIElement
    {
        protected override GUIStyle BaseStyle => EditorStyles.label;

        public LabelField(string text) : base(text) { }
        public LabelField(string text, string tooltip) : base(text, tooltip) { }

        protected override void Display(Rect position)
        {
            EditorGUI.LabelField(position, Content, GUIStyle);
        }
    }
}