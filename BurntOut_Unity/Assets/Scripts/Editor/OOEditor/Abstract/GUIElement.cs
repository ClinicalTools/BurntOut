using UnityEngine;

namespace OOEditor
{
    public abstract class GUIElement
    {
        protected static int elementNums;

        protected string Name { get; set; }
        public bool Selected
        {
            get
            {
                return GUI.GetNameOfFocusedControl() == Name;
            }
        }

        public GUIContent Content { get; set; }
        public float MinWidth { get; set; }
        public float Width { get; set; }
        public float MaxWidth { get; set; }
        private readonly EditorStyle style = new EditorStyle();
        public EditorStyle Style
        {
            get { return style; }
        }

        public virtual GUIStyle GUIStyle
        {
            get
            {
                var guiStyle = new GUIStyle(BaseStyle);

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

        protected GUIElement()
        {
            Name = "" + elementNums++;
        }
        protected GUIElement(string text)
        {
            Name = "" + elementNums++;
            Content = new GUIContent(text);
        }
        protected GUIElement(string text, string tooltip)
        {
            Name = "" + elementNums++;
            Content = new GUIContent(text, tooltip);
        }


        public virtual void Draw()
        {
            OOEditorManager.DrawGuiElement(this, PrepareDisplay, Content);
        }

        protected virtual void PrepareDisplay(Rect position)
        {
            GUI.SetNextControlName(Name);
            Display(position);
        }

        protected abstract void Display(Rect position);
    }
}
