using UnityEngine;

namespace OOEditor
{
    public abstract class FoldoutClassDrawer<T> : ClassDrawer<T>
    {
        public virtual bool Expanded
        {
            get { return Foldout.Value; }
            set { Foldout.Value = value; }
        }
        protected virtual Color? FoldoutColor { get; } = null;

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