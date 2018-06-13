using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public interface IGUIObjectDrawer<T>
    {
        T Value { get; set; }
        void Draw();
    }
}