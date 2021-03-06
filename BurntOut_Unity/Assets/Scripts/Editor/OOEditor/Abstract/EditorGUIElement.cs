﻿using OOEditor.Internal;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Basic class for Editor GUI elements.
    /// </summary>
    public abstract class EditorGUIElement
    {
        // 4,294,967,295 elements should be enough
        // Note that this is shared between all instances of EditorGUIElement, not just those in a particular window
        protected static uint ElementNums { get; set; } = 0;

        public string Name { get; protected set; }

        /// <summary>
        /// True if the control has focus
        /// </summary>
        public bool Focused { get; private set; }
        /// <summary>
        /// Refreshes whether the element is focused.
        /// Should be called once at the start of PrepareDisplay.
        /// </summary>
        protected void UpdateFocused()
        {
            Focused = OOEditorManager.FocusedControlName.Contains(Name);
        }

        /// <summary>
        /// Text and tooltip for the element
        /// </summary>
        public virtual GUIContent Content { get; }

        private float minWidth;
        /// <summary>
        /// Setting is based on a font-size of 11, but getting is scaled by the current font-size.
        /// 0 represents no minimum.
        /// </summary>
        public virtual float MinWidth
        {
            get { return OOEditorManager.ScaledWidth(minWidth, GUIStyle?.fontSize ?? 0); }
            set { minWidth = value; }
        }
        private float maxWidth;
        /// <summary>
        /// Setting is based on a font-size of 11, but getting is scaled by the current font-size.
        /// 0 represents no maximum.
        /// </summary>
        public virtual float MaxWidth
        {
            get { return OOEditorManager.ScaledWidth(maxWidth, GUIStyle?.fontSize ?? 0); }
            set { maxWidth = value; }
        }
        private bool? fitWidth;
        /// <summary>
        /// True if the control should fit its content's width.
        /// </summary>
        public virtual bool FitWidth
        {
            get { return fitWidth ?? DefaultFitWidth; }
            set { fitWidth = value; }
        }
        protected virtual bool DefaultFitWidth => OOEditorManager.InToolbar;
        /// <summary>
        /// Default width to draw the element at.
        /// 0 represents no default width.
        /// </summary>
        public virtual float Width
        {
            get
            {
                if (FitWidth && Content != null)
                {
                    var width = GUIStyle.CalcSize(Content).x;
                    if (maxWidth > 0)
                        return Mathf.Clamp(width, MinWidth, MaxWidth);
                    else
                        return Mathf.Max(width, MinWidth);
                }
                return 0;
            }
        }

        /// <summary>
        /// The style to display the element text in.
        /// </summary>
        public EditorStyle Style { get; } = new EditorStyle();

        /// <summary>
        /// Internal use only. The style to draw this element in.
        /// </summary>
        public GUIStyle GUIStyle { get; protected set; }
        /// <summary>
        /// Resets the value of GUIStyle.
        /// </summary>
        /// <remarks>
        /// GUIStyle can be called a few times per draw call, so I elected to set it once per draw, 
        /// rather than put it in the getter.
        /// </remarks>
        protected virtual void ResetGUIStyle()
        {
            if (OOEditorManager.InToolbar)
                GUIStyle = new GUIStyle(ToolbarStyle);
            else
                GUIStyle = new GUIStyle(BaseStyle);

            if (Focused && !OOEditorManager.InToolbar)
            {
                GUIStyle.normal = GUIStyle.focused;
                GUIStyle.onNormal = GUIStyle.onFocused;
            }


            // Basic elements treated like labels
            if (OOEditorManager.OverrideLabelStyle != null)
                OOEditorManager.OverrideLabelStyle.ApplyToStyle(GUIStyle);
            if (OOEditorManager.OverrideTextStyle != null)
                OOEditorManager.OverrideTextStyle.ApplyToStyle(GUIStyle);
            
            Style.ApplyToStyle(GUIStyle);
        }


        protected abstract GUIStyle BaseStyle { get; }
        protected virtual GUIStyle ToolbarStyle => BaseStyle;

        protected EditorGUIElement(string text = null, string tooltip = null, Texture image = null)
        {
            Content = new GUIContent();

            if (text != null)
                Content.text = text;
            if (tooltip != null)
                Content.tooltip = tooltip;
            if (image != null)
                Content.image = image;


            Name = (ElementNums++).ToString("X8");
        }

        /// <summary>
        /// Draws the GUI element.
        /// </summary>
        public virtual void Draw()
        {
            ResetGUIStyle();
            OOEditorManager.DrawGuiElement(this, PrepareDisplay, Content);
        }

        /// <summary>
        /// Stores the last valid width passed to <see cref="PrepareDisplay(Rect)"/>.
        /// 
        /// In every other OnGUI call, elements seem to be given a width of 1, which can cause issues.
        /// </summary>
        protected float ValidWidth { get; set; }

        /// <summary>
        /// Checks whether the width is valid, and sets the control's name.
        /// </summary>
        /// <param name="position">Position to draw in.</param>
        protected virtual void PrepareDisplay(Rect position)
        {
            UpdateFocused();

            if (position.width > 1)
                ValidWidth = position.width;

            // Setting name is important for checking if focused
            GUI.SetNextControlName(Name);

            Display(position);
        }

        /// <summary>
        /// Draws the GUI element in a passed rectangle. 
        /// Not always called immediately after <see cref="Draw"/>.
        /// </summary>
        /// <param name="position">Position to draw the element in.</param>
        protected abstract void Display(Rect position);
    }
}
