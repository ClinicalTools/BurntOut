using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public abstract class GUIObjectDrawer<T>
    {
        public virtual T Value { get; set; }
        public abstract void Init(T val);
        public abstract void Draw();
    }
}