using System;
using UnityEditor;
using UnityEngine;

namespace CtiEditor.Disposable
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group.
    /// </summary>
    public class FontSize : IDisposable
    {
        private readonly int foldoutFontSize, labelFontSize, numberFieldFontSize, objectFieldFontSize,
            popupFontSize, textAreaFontSize, textFieldFontSize, toggleFontSize;

        public FontSize(int fontSize)
        {
            foldoutFontSize = EditorStyles.foldout.fontSize;
            labelFontSize = EditorStyles.label.fontSize;
            numberFieldFontSize = EditorStyles.numberField.fontSize;
            objectFieldFontSize = EditorStyles.objectField.fontSize;
            popupFontSize = EditorStyles.popup.fontSize;
            textAreaFontSize = EditorStyles.textArea.fontSize;
            textFieldFontSize = EditorStyles.textField.fontSize;
            toggleFontSize = EditorStyles.toggle.fontSize;

            EditorStyles.foldout.fontSize = fontSize;
            EditorStyles.label.fontSize = fontSize;
            EditorStyles.numberField.fontSize = fontSize;
            EditorStyles.objectField.fontSize = fontSize;
            EditorStyles.popup.fontSize = fontSize;
            EditorStyles.textArea.fontSize = fontSize;
            EditorStyles.textField.fontSize = fontSize;
            EditorStyles.toggle.fontSize = fontSize;
        }

        public void Dispose()
        {
            EditorStyles.foldout.fontSize = foldoutFontSize;
            EditorStyles.label.fontSize = labelFontSize;
            EditorStyles.numberField.fontSize = numberFieldFontSize;
            EditorStyles.objectField.fontSize = objectFieldFontSize;
            EditorStyles.popup.fontSize = popupFontSize;
            EditorStyles.textArea.fontSize = textAreaFontSize;
            EditorStyles.textField.fontSize = textFieldFontSize;
            EditorStyles.toggle.fontSize = toggleFontSize;
        }
    }
}