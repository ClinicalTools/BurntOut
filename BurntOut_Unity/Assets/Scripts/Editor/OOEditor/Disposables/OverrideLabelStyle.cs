using OOEditor.Internal;
using System;
using UnityEngine;

namespace OOEditor
{
    public class OverrideLabelStyle : EditorStyle
    {
        private class DisposableOverrideLabelStyle : IDisposable
        {
            private bool disposed;
            private readonly EditorStyle oldStyle;

            public DisposableOverrideLabelStyle(EditorStyle style)
            {
                oldStyle = OOEditorManager.OverrideLabelStyle;

                OOEditorManager.OverrideLabelStyle = style;
            }

            public void Dispose()
            {
                if (disposed)
                    Debug.LogError("OverrideLabelStyle incorrectly disposed.");
                else
                    OOEditorManager.OverrideLabelStyle = oldStyle;

                disposed = true;
            }
        }

        public OverrideLabelStyle() { }
        public OverrideLabelStyle(Color fontColor) : base(fontColor) { }
        public OverrideLabelStyle(FontStyle fontStyle) : base(fontStyle) { }
        public OverrideLabelStyle(FontStyle fontStyle, Color fontColor) : base(fontStyle, fontColor) { }
        public OverrideLabelStyle(int fontSize) : base(fontSize) { }
        public OverrideLabelStyle(int fontSize, FontStyle fontStyle) : base(fontSize, fontStyle) { }
        public OverrideLabelStyle(int fontSize, FontStyle fontStyle, Color fontColor)
            : base(fontSize, fontStyle, fontColor) { }
        public OverrideLabelStyle(EditorStyle style)
        {
            FontSize = style.FontSize;
            FontStyle = style.FontStyle;
            FontColor = style.FontColor;
        }


        private IDisposable disposable;
        public IDisposable Draw()
        {
            disposable = new DisposableOverrideLabelStyle(this);
            return disposable;
        }

        public void EndDraw()
        {
            if (disposable == null)
                Debug.LogError("OverrideLabelStyle incorrectly disposed.");
            else
                disposable.Dispose();
        }
    }
}