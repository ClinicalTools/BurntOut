using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public abstract class FoldoutObjectDrawer<T> : IGUIObjectDrawer<T>
    {
        public virtual T Value { get; set; }
        public virtual bool Expanded
        {
            get { return Foldout.Value; }
            set { Foldout.Value = value; }
        }
        protected virtual Color? FoldoutColor { get; } = null;

        public abstract void Draw();

        protected Foldout Foldout { get; } = new Foldout(" ");

        protected abstract string FoldoutName { get; }

        public virtual void DrawFoldout()
        {
            Foldout.Content.text = FoldoutName;
            if (FoldoutColor != null)
                Foldout.Style.FontColor = FoldoutColor;
            Foldout.Draw();
        }
    }
}