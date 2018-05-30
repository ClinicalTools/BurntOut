using System;
using UnityEditor;

namespace CtiEditor.Disposable
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group.
    /// </summary>
    internal class LabelFontStyle : IDisposable
    {
        private readonly UnityEngine.FontStyle foldoutFontStyle, labelFontStyle;

        internal LabelFontStyle(UnityEngine.FontStyle fontStyle)
        {
            foldoutFontStyle = EditorStyles.foldout.fontStyle;
            labelFontStyle = EditorStyles.label.fontStyle;

            EditorStyles.foldout.fontStyle = fontStyle;
            EditorStyles.label.fontStyle = fontStyle;
        }

        public void Dispose()
        {
            EditorStyles.foldout.fontStyle = foldoutFontStyle;
            EditorStyles.label.fontStyle = labelFontStyle;
        }
    }
}