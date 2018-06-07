﻿
using System;
using UnityEngine;

namespace OOEditor
{
    public abstract class GUIControl<T> : GUIElement
    {
        public abstract T Value { get; set; }

        public event EventHandler Changed;

        public override GUIStyle GUIStyle
        {
            get
            {
                var guiStyle = new GUIStyle(BaseStyle);

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
                Changed(this, null);
        }

        protected GUIControl() : base() { }
        protected GUIControl(string text) : base(text) { }
        protected GUIControl(string text, string tooltip) : base(text, tooltip) { }
    }
}