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
                // If the value has changed, call the changed event
                if (!(value?.Equals(this.value) ?? (this.value == null)))
                {
                    this.value = value;
                    var changedArgs = new ControlChangedArgs<T>(this.value, value);
                    Changed?.Invoke(this, changedArgs);
                    OOEditorManager.ElementChanged(this, changedArgs);
                }
            }
        }

        /// <summary>
        /// Occurs when the control's value changes.
        /// </summary>
        public event EventHandler<ControlChangedArgs<T>> Changed;

        protected override void ResetGUIStyle()
        {
            if (OOEditorManager.InToolbar)
                GUIStyle = new GUIStyle(ToolbarStyle);
            else
                GUIStyle = new GUIStyle(BaseStyle);

            if (Focused && !OOEditorManager.InToolbar)
                GUIStyle.normal = GUIStyle.focused;

            if (OOEditorManager.OverrideTextStyle != null)
                OOEditorManager.OverrideTextStyle.ApplyToStyle(GUIStyle);

            Style.ApplyToStyle(GUIStyle);
        }

        protected GUIControl(string text = null, string tooltip = null, Texture image = null) 
            : base(text, tooltip, image) { }

        /// <summary>
        /// Draws the control.
        /// </summary>
        public override void Draw()
        {
            ResetGUIStyle();
            OOEditorManager.DrawGuiElement(this, PrepareDisplay, Content);
        }

        /// <summary>
        /// Updates the control's value and then draws it.
        /// </summary>
        /// <param name="value">Updated value for the control.</param>
        public virtual void Draw(T value)
        {
            this.value = value;

            Draw();
        }

    }
}