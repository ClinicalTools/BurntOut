using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public interface GUIObjectDrawer<T>
    {
        T Value { get; set; }
        void Draw();
    }
}