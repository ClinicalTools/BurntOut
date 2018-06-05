using System;
using UnityEngine;

namespace OOEditor
{
    public class OverrideTextStyle : EditorStyle, IDisposable
    {
        private OverrideTextStyle oldLabelStyle;
        public OverrideTextStyle()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}