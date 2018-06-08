using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class IntField : GUIControlField<int>
    {
        public override int Value { get; set; }

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