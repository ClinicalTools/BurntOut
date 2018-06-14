using OOEditor.Internal;
using System;
using UnityEngine;

namespace OOEditor
{
    public abstract class GUIControl<T> : EditorGUIElement, IGUIObjectDrawer<T>
    {
        private T value;
        public virtual T Value
        {
            get
            {
                return value;
            }
            set
            {
                if (!value.Equals(this.value) && !firstDraw)
                    Changed?.Invoke(this, new ControlChangedArgs<T>(value, this.value));

                this.value = value;
            }
        }

        public event EventHandler<ControlChangedArgs<T>> Changed;

        public override GUIStyle GUIStyle
        {
            get
            {
                GUIStyle guiStyle;
                if (OOEditorManager.InToolbar)
                    guiStyle = new GUIStyle(ToolbarStyle);
                else
                    guiStyle = new GUIStyle(BaseStyle);

                if (Focused && !OOEditorManager.InToolbar)
                    guiStyle.normal = guiStyle.focused;

                Style.ApplyToStyle(guiStyle);
                if (OOEditorManager.OverrideTextStyle != null)
                    OOEditorManager.OverrideTextStyle.ApplyToStyle(guiStyle);

                return guiStyle;
            }
        }

        protected GUIControl() : base() { }
        protected GUIControl(string text) : base(text) { }
        protected GUIControl(string text, string tooltip) : base(text, tooltip) { }

        private bool firstDraw = true;
        public override void Draw()
        {
            // First draw calls changed, since it changed from its default
            if (firstDraw)
            {
                Changed?.Invoke(this, new ControlChangedArgs<T>(default(T), Value));
                firstDraw = false;
            }
            OOEditorManager.DrawGuiElement(this, PrepareDisplay, Content);
        }

        protected override void PrepareDisplay(Rect position)
        {
            var oldVal = Value;

            base.PrepareDisplay(position);

            if (!Value.Equals(oldVal) && Changed != null)
                Changed(this, new ControlChangedArgs<T>(oldVal, Value));
        }

    }
}