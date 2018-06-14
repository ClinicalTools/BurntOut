using OOEditor.Internal;
using UnityEngine;

namespace OOEditor
{
    public abstract class EditorGUIElement
    {
        // 4,294,967,295 elements should be enough
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

        public GUIContent Content { get; protected set; }
        public float MinWidth { get; set; }
        public float Width { get; set; }
        public float MaxWidth { get; set; }
        /// <summary>
        /// Stores the last valid width of the element.
        /// 
        /// In every other OnGUI call, elements seem to be given a width of 1, which can cause issues.
        /// </summary>
        protected float ValidWidth { get; set; }
        public EditorStyle Style { get; } = new EditorStyle();

        public virtual GUIStyle GUIStyle
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
                // Basic elements treated like labels
                if (OOEditorManager.OverrideLabelStyle != null)
                    OOEditorManager.OverrideLabelStyle.ApplyToStyle(guiStyle);
                if (OOEditorManager.OverrideTextStyle != null)
                    OOEditorManager.OverrideTextStyle.ApplyToStyle(guiStyle);

                return guiStyle;
            }
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
