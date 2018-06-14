using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public interface IGUIObjectDrawer<T>
    {
        event EventHandler<ControlChangedArgs<T>> Changed;

        T Value { get; set; }
        void Draw();
    }
}