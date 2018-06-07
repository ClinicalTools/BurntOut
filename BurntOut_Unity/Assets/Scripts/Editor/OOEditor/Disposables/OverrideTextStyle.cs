using System;
using UnityEngine;

namespace OOEditor
{
    public class OverrideTextStyle : EditorStyle, IDisposable
    {
        private OverrideTextStyle oldStyle;
        public OverrideTextStyle() : base()
        {
            oldStyle = OOEditorManager.OverrideTextStyle;
            OOEditorManager.OverrideTextStyle = this;
        }
        public OverrideTextStyle(int fontSize) : base(fontSize)
        {
            oldStyle = OOEditorManager.OverrideTextStyle;
            OOEditorManager.OverrideTextStyle = this;
        }
        public OverrideTextStyle(FontStyle fontStyle) : base(fontStyle)
        {
            oldStyle = OOEditorManager.OverrideTextStyle;
            OOEditorManager.OverrideTextStyle = this;
        }
        public OverrideTextStyle(int fontSize, FontStyle fontStyle) : base(fontSize, fontStyle)
        {
            oldStyle = OOEditorManager.OverrideTextStyle;
            OOEditorManager.OverrideTextStyle = this;
        }
        public OverrideTextStyle(EditorStyle style) : base()
        {
            FontSize = style.FontSize;
            FontStyle = style.FontStyle;
        }

        public void Dispose()
        {
            OOEditorManager.OverrideTextStyle = oldStyle;
        }
    }
}