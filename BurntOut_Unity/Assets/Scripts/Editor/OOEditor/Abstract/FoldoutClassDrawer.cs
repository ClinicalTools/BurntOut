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

        protected abstract Foldout Foldout { get; }

        public FoldoutClassDrawer(T value) : base(value) { }

        public virtual void DrawFoldout()
        {
            Foldout.Draw();
        }
    }
}