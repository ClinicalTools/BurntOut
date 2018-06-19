using OOEditor.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public class OverrideTextStyle : EditorStyle
    {
        private class DisposableOverrideTextStyle : IDisposable
        {
            private bool disposed;
            private readonly EditorStyle oldStyle;

            public DisposableOverrideTextStyle(EditorStyle style)
            {
                oldStyle = OOEditorManager.OverrideTextStyle;

                OOEditorManager.OverrideTextStyle = style;
            }

            public void Dispose()
            {
                if (disposed)
                    Debug.LogError("OverrideTextStyle incorrectly disposed.");
                else
                    OOEditorManager.OverrideTextStyle = oldStyle;

                disposed = true;
            }
        }

        public OverrideTextStyle() { }
        public OverrideTextStyle(Color fontColor) : base(fontColor) { }
        public OverrideTextStyle(FontStyle fontStyle) : base(fontStyle) { }
        public OverrideTextStyle(FontStyle fontStyle, Color fontColor) : base(fontStyle, fontColor) { }
        public OverrideTextStyle(int fontSize) : base(fontSize) { }
        public OverrideTextStyle(int fontSize, FontStyle fontStyle) : base(fontSize, fontStyle) { }
        public OverrideTextStyle(int fontSize, FontStyle fontStyle, Color fontColor) 
            : base(fontSize, fontStyle, fontColor) { }
        public OverrideTextStyle(EditorStyle style)
        {
            FontSize = style.FontSize;
            FontStyle = style.FontStyle;
            FontColor = style.FontColor;
        }


        private IDisposable disposable;
        public IDisposable Draw()
        {
            disposable = new DisposableOverrideTextStyle(this);
            return disposable;
        }

        public void EndDraw()
        {
            if (disposable == null)
                Debug.LogError("OverrideTextStyle incorrectly disposed.");
            else
                disposable.Dispose();
        }
    }
}