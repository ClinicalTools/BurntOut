
using UnityEngine;

namespace OOEditor
{
    public abstract class GUIControl<T> : GUIElement
    {
        public abstract T Value { get; set; }

        public override GUIStyle GUIStyle
        {
            get
            {
                var guiStyle = new GUIStyle(BaseStyle);

                Style.ApplyToStyle(guiStyle);
                if (OOEditorManager.OverrideTextStyle != null)
                    OOEditorManager.OverrideTextStyle.ApplyToStyle(guiStyle);

                return guiStyle;
            }
        }

        protected GUIControl() : base() { }
        protected GUIControl(string text) : base(text) { }
        protected GUIControl(string text, string tooltip) : base(text, tooltip) { }
    }
}