using UnityEngine;

namespace OOEditor
{
    public abstract class GUIElement
    {
        public GUIContent Content { get; set; }
        public float MinWidth { get; set; }
        public float Width { get; set; }
        public float MaxWidth { get; set; }
        private readonly EditorStyle style = new EditorStyle();
        public EditorStyle Style
        {
            get { return style; }
        }

        public GUIStyle GUIStyle {
            get
            {
                var guiStyle = new GUIStyle(BaseStyle);

                Style.ApplyToStyle(guiStyle);
                OOEditorManager.OverrideLabelStyle.ApplyToStyle(guiStyle);
                OOEditorManager.OverrideTextStyle.ApplyToStyle(guiStyle);

                return guiStyle;
            }
        }

        internal abstract GUIStyle BaseStyle { get; }

        protected GUIElement()
        {

        }
        protected GUIElement(string text)
        {
            Content = new GUIContent(text);
        }
        protected GUIElement(string text, string tooltip)
        {
            Content = new GUIContent(text, tooltip);
        }


        public virtual void Draw()
        {
            OOEditorManager.DrawGuiElement(this, Display, Content);
        }

        protected abstract void Display(Rect position);
    }
}
