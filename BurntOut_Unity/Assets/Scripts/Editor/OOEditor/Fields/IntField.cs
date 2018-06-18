using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class IntField : GUIControlField<int>
    {
        protected override GUIStyle BaseStyle => EditorStyles.numberField;
        protected override GUIStyle ToolbarStyle => EditorStyles.toolbarTextField;
        
        protected override float ReservedWidth { get; } = 10;

        public IntField(int value) : base()
        {
            Value = value;
        }
        public IntField(int value, string text) : base(text)
        {
            Value = value;
        }
        public IntField(int value, string text, string tooltip) : base(text, tooltip)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            Value = EditorGUI.IntField(position, Value, GUIStyle);
        }
    }
}