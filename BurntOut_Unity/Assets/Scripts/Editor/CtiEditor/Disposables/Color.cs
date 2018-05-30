using System;
using UnityEngine;

namespace CtiEditor.Disposable
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group.
    /// </summary>
    internal class Color : IDisposable
    {
        private readonly UnityEngine.Color lastColor;

        internal Color(UnityEngine.Color color)
        {
            lastColor = GUI.contentColor;
            GUI.contentColor = color;
        }

        public void Dispose()
        {
            GUI.contentColor = lastColor;
        }
    }
}