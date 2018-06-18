using OOEditor.Internal;
using System;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A GUI element that stores a value.
    /// </summary>
    /// <typeparam name="T">Type of value to store</typeparam>
    public abstract class GUIControl<T> : EditorGUIElement, IGUIObjectDrawer<T>
    {
        private T value;
        /// <summary>
        /// Value being represented by the control.
        /// </summary>
        public virtual T Value
        {
            get
            {
                return value;
            }
            set
            {
                // If the value has changed, and the control has been drawn at least once, 
                //  call the changed event
                if (!value.Equals(this.value) && !firstDraw)
                    Changed?.Invoke(this, new ControlChangedArgs<T>(this.value, value));

                this.value = value;
            }
        }

        /// <summary>
        /// Occurs when the control's value changes.
        /// </summary>
        /// <remarks>Not called before the first draw.</remarks>
        public event EventHandler<ControlChangedArgs<T>> Changed;

        protected override void ResetGUIStyle()
        {
            if (OOEditorManager.InToolbar)
                GUIStyle = new GUIStyle(ToolbarStyle);
            else
                GUIStyle = new GUIStyle(BaseStyle);

            if (Focused && !OOEditorManager.InToolbar)
                GUIStyle.normal = GUIStyle.focused;

            Style.ApplyToStyle(GUIStyle);
            if (OOEditorManager.OverrideTextStyle != null)
                OOEditorManager.OverrideTextStyle.ApplyToStyle(GUIStyle);
        }

        protected GUIControl() : base() { }
        protected GUIControl(string text) : base(text) { }
        protected GUIControl(string text, string tooltip) : base(text, tooltip) { }

        private bool firstDraw = true;
        /// <summary>
        /// Draws the control.
        /// </summary>
        public override void Draw()
        {
            ResetGUIStyle();

            // First draw calls changed, since it changed from its default
            if (firstDraw)
            {
                Changed?.Invoke(this, new ControlChangedArgs<T>(default(T), Value));
                firstDraw = false;
            }
            OOEditorManager.DrawGuiElement(this, PrepareDisplay, Content);
        }

        /// <summary>
        /// Updates the control's value and then draws it.
        /// </summary>
        /// <param name="value">Updated value for the control</param>
        public void Draw(T value)
        {
            this.value = value;

            Draw();
        }

    }
}