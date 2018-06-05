using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public abstract class GUIControlField<T> : GUIControl<T>
    {
        EditorStyle labelStyle = new EditorStyle();
        public EditorStyle LabelStyle
        {
            get { return labelStyle; }
        }

        protected abstract float AbsoluteMinWidth { get; }

        protected GUIControlField() : base() { }
        protected GUIControlField(string tooltip) : base(tooltip) { }
        protected GUIControlField(string text, string tooltip) : base(text, tooltip) { }

        public override void Draw()
        {
            OOEditorManager.DrawGuiElement(this, DisplayLabel, Content);
        }

        // Draws the label portion of the control
        private void DisplayLabel(Rect position)
        {
            if (Content != null)
            {
                // Ensure width is at least as big as the larger of the minimum widths
                float minWidth = Mathf.Max(AbsoluteMinWidth, MinWidth);

                // Typically use labelWidth for the width, but ensure at least MinWidth pixels are saved for the field
                var width = Mathf.Min(EditorGUIUtility.labelWidth, position.width - minWidth);
                Rect labelRect = new Rect(position.x, position.y, width, position.height);

                EditorGUI.LabelField(labelRect, Content, OOEditorManager.GetLabelStyle(LabelStyle));
                position.x += width;
                position.width -= width;
            }

            if (position.width > 0)
                Display(position);
        }
    }
}