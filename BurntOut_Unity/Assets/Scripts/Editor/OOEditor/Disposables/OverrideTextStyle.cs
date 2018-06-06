using System;
using UnityEngine;

namespace OOEditor
{
    public class OverrideTextStyle : EditorStyle, IDisposable
    {
        private OverrideTextStyle oldStyle;
        public OverrideTextStyle()
        {
            oldStyle = OOEditorManager.OverrideTextStyle;
            OOEditorManager.OverrideTextStyle = this;
        }

        public void Dispose()
        {
            OOEditorManager.OverrideTextStyle = oldStyle;
        }
    }
}