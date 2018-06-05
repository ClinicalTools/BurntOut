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

        public virtual GUIStyle GUILabelStyle
        {
            get
            {
                var guiStyle = new GUIStyle(EditorStyles.label);

                Style.ApplyToStyle(guiStyle);
                LabelStyle.ApplyToStyle(guiStyle);
                if (OOEditorManager.OverrideTextStyle != null)
                    OOEditorManager.OverrideLabelStyle.ApplyToStyle(guiStyle);
                if (OOEditorManager.OverrideTextStyle != null)
                    OOEditorManager.OverrideTextStyle.ApplyToStyle(guiStyle);

                return guiStyle;
            }
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
                var color = GUI.contentColor;
                // Ensure width is at least as big as the larger of the minimum widths
                float minWidth = Mathf.Max(AbsoluteMinWidth, MinWidth);

                // Typically use labelWidth for the width, but ensure at least MinWidth pixels are saved for the field
                var width = Mathf.Min(EditorGUIUtility.labelWidth, position.width - minWidth);
                Rect labelRect = new Rect(position.x, position.y, width, position.height);

                if (GUI.contentColor == Color.white && Selected)
                    GUI.contentColor = new Color(0.425f, .7f, 1.42f);

                GUI.SetNextControlName(Name);
                EditorGUI.LabelField(labelRect, Content, GUILabelStyle);
                position.x += width;
                position.width -= width;

                GUI.contentColor = color;
            }

            GUI.SetNextControlName(Name);
            if (position.width > 0)
                Display(position);
        }
    }
}