﻿using System;

namespace OOEditor
{
    public class OverrideLabelStyle : EditorStyle, IDisposable 
    {
        private OverrideLabelStyle oldLabelStyle;
        public OverrideLabelStyle()
        {

        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}