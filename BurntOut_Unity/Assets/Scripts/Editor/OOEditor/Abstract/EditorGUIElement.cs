using OOEditor.Internal;
using UnityEngine;

namespace OOEditor
{
    public abstract class EditorGUIElement
    {
        // 4,294,967,295 elements should be enough
        // Note that this is shared between all instances of EditorGUIElement, not just those in a particular window
        protected static uint elementNums = 0;

        protected string Name { get; set; }

        private string focusedControl = "";
        protected string FocusedControlName
        {
            get
            {
                var control = GUI.GetNameOfFocusedControl();
                if (!string.IsNullOrEmpty(control))
                    focusedControl = control;
                return focusedControl;
            }
        }
        public bool Focused => FocusedControlName.Contains(Name);

        public virtual GUIContent Content { get; }

        private float minWidth;
        public virtual float MinWidth
        {
            get { return OOEditorManager.ScaledWidth(minWidth, GUIStyle?.fontSize ?? 0); }
            set { minWidth = value; }
        }
        private float maxWidth;
        public virtual float MaxWidth
        {
            get { return OOEditorManager.ScaledWidth(maxWidth, GUIStyle?.fontSize ?? 0); }
            set { maxWidth = value; }
        }
        private bool? fitWidth;
        public virtual bool FitWidth
        {
            get { return fitWidth ?? DefaultFitWidth; }
            set { fitWidth = value; }
        }
        protected virtual bool DefaultFitWidth => OOEditorManager.InToolbar;
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
        /// Stores the last valid width of the element.
        /// 
        /// In every other OnGUI call, elements seem to be given a width of 1, which can cause issues.
        /// </summary>
        protected float ValidWidth { get; set; }
        public EditorStyle Style { get; } = new EditorStyle();

        public GUIStyle GUIStyle { get; protected set; }
        protected virtual void ResetGUIStyle()
        {
            if (OOEditorManager.InToolbar)
                GUIStyle = new GUIStyle(ToolbarStyle);
            else
                GUIStyle = new GUIStyle(BaseStyle);

            if (Focused && !OOEditorManager.InToolbar)
                GUIStyle.normal = GUIStyle.focused;

            Style.ApplyToStyle(GUIStyle);
            // Basic elements treated like labels
            if (OOEditorManager.OverrideLabelStyle != null)
                OOEditorManager.OverrideLabelStyle.ApplyToStyle(GUIStyle);
            if (OOEditorManager.OverrideTextStyle != null)
                OOEditorManager.OverrideTextStyle.ApplyToStyle(GUIStyle);
        }


        protected abstract GUIStyle BaseStyle { get; }
        protected virtual GUIStyle ToolbarStyle => BaseStyle;

        protected EditorGUIElement()
        {
            Name = (elementNums++).ToString("X8");
        }
        protected EditorGUIElement(string text)
        {
            Name = (elementNums++).ToString("X8");
            Content = new GUIContent(text);
        }
        protected EditorGUIElement(string text, string tooltip)
        {
            Name = (elementNums++).ToString("X8");
            Content = new GUIContent(text, tooltip);
        }


        public virtual void Draw()
        {
            ResetGUIStyle();
            OOEditorManager.DrawGuiElement(this, PrepareDisplay, Content);
        }

        protected virtual void PrepareDisplay(Rect position)
        {
            if (position.width > 1)
                ValidWidth = position.width;

            GUI.SetNextControlName(Name);
            Display(position);
        }

        protected abstract void Display(Rect position);
    }
}
