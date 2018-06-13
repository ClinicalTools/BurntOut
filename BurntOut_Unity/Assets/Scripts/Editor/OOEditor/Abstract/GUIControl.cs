using OOEditor.Internal;
using System;
using UnityEngine;

namespace OOEditor
{
    public abstract class GUIControl<T> : GUIElement, GUIObjectDrawer<T>
    {
        public virtual T Value { get; set; }

        public event EventHandler<ControlChangedArgs<T>> Changed;

        public override GUIStyle GUIStyle
        {
            get
            {
                GUIStyle guiStyle;
                if (OOEditorManager.InToolbar == 0)
                    guiStyle = new GUIStyle(BaseStyle);
                else
                    guiStyle = new GUIStyle(ToolbarStyle);

                if (Focused && OOEditorManager.InToolbar == 0)
                    guiStyle.normal = guiStyle.focused;

                Style.ApplyToStyle(guiStyle);
                if (OOEditorManager.OverrideTextStyle != null)
                    OOEditorManager.OverrideTextStyle.ApplyToStyle(guiStyle);

                return guiStyle;
            }
        }

        protected override void PrepareDisplay(Rect position)
        {
            var oldVal = Value;

            base.PrepareDisplay(position);

            if (!Value.Equals(oldVal) && Changed != null)
                Changed(this, new ControlChangedArgs<T>(oldVal, Value));
        }

        protected GUIControl() : base() { }
        protected GUIControl(string text) : base(text) { }
        protected GUIControl(string text, string tooltip) : base(text, tooltip) { }
    }
}