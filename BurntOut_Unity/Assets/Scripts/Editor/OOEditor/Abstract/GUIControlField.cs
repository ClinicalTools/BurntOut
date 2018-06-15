using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public abstract class GUIControlField<T> : GUIControl<T>
    {
        public EditorStyle LabelStyle { get; } = new EditorStyle();

        // Width cannot be estimated when the element is using the content for a label
        public override float Width { get; } = 0;

        public GUIStyle GUILabelStyle { get; protected set; }
        protected override void ResetGUIStyle()
        {
            base.ResetGUIStyle();

            
            GUILabelStyle = new GUIStyle(EditorStyles.label);
            if (Focused)
                GUILabelStyle.normal = GUILabelStyle.focused;

            Style.ApplyToStyle(GUILabelStyle);
            LabelStyle.ApplyToStyle(GUILabelStyle);
            if (OOEditorManager.OverrideLabelStyle != null)
                OOEditorManager.OverrideLabelStyle.ApplyToStyle(GUILabelStyle);
            if (OOEditorManager.OverrideTextStyle != null)
                OOEditorManager.OverrideTextStyle.ApplyToStyle(GUILabelStyle);
        }

        protected abstract float AbsoluteMinWidth { get; }

        protected GUIControlField() : base() { }
        protected GUIControlField(string tooltip) : base(tooltip) { }
        protected GUIControlField(string text, string tooltip) : base(text, tooltip) { }

        // Draws the label portion of the control
        protected override void PrepareDisplay(Rect position)
        {
            if (Content != null)
            {
                // Ensure width is at least as big as the larger of the minimum widths
                float minWidth = Mathf.Max(AbsoluteMinWidth, MinWidth);

                // Typically use labelWidth for the width, but ensure at least MinWidth pixels are saved for the field
                var width = Mathf.Min(EditorGUIUtility.labelWidth, position.width - minWidth);
                // Width cannot be less than 0
                width = Mathf.Max(width, 1);
                Rect labelRect = new Rect(position.x, position.y, width, position.height);

                GUI.SetNextControlName(Name + "label");
                EditorGUI.LabelField(labelRect, Content, GUILabelStyle);

                if (Event.current.rawType == EventType.MouseDown &&
                    labelRect.Contains(Event.current.mousePosition))
                {
                    GUI.FocusControl(Name + "label");

                    EditorWindow.focusedWindow.Repaint();
                }

                position.x += width;
                position.width -= width;
            }

            base.PrepareDisplay(position);
        }
    }
}