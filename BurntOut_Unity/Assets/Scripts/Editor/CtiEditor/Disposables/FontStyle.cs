using System;
using UnityEditor;

namespace CtiEditor.Disposable
{
    /// <summary>
    /// Class to keep contained elements within an EditorGUILayout horizontal group.
    /// </summary>
    internal class FontStyle : IDisposable
    {
        private readonly UnityEngine.FontStyle foldoutFontStyle, labelFontStyle, numberFieldFontStyle, objectFieldFontStyle,
            popupFontStyle, textAreaFontStyle, textFieldFontStyle, toggleFontStyle;

        internal FontStyle(UnityEngine.FontStyle fontStyle)
        {
            foldoutFontStyle = EditorStyles.foldout.fontStyle;
            labelFontStyle = EditorStyles.label.fontStyle;
            numberFieldFontStyle = EditorStyles.numberField.fontStyle;
            objectFieldFontStyle = EditorStyles.objectField.fontStyle;
            popupFontStyle = EditorStyles.popup.fontStyle;
            textAreaFontStyle = EditorStyles.textArea.fontStyle;
            textFieldFontStyle = EditorStyles.textField.fontStyle;
            toggleFontStyle = EditorStyles.toggle.fontStyle;

            EditorStyles.foldout.fontStyle = fontStyle;
            EditorStyles.label.fontStyle = fontStyle;
            EditorStyles.numberField.fontStyle = fontStyle;
            EditorStyles.objectField.fontStyle = fontStyle;
            EditorStyles.popup.fontStyle = fontStyle;
            EditorStyles.textArea.fontStyle = fontStyle;
            EditorStyles.textField.fontStyle = fontStyle;
            EditorStyles.toggle.fontStyle = fontStyle;
        }

        public void Dispose()
        {
            EditorStyles.foldout.fontStyle = foldoutFontStyle;
            EditorStyles.label.fontStyle = labelFontStyle;
            EditorStyles.numberField.fontStyle = numberFieldFontStyle;
            EditorStyles.objectField.fontStyle = objectFieldFontStyle;
            EditorStyles.popup.fontStyle = popupFontStyle;
            EditorStyles.textArea.fontStyle = textAreaFontStyle;
            EditorStyles.textField.fontStyle = textFieldFontStyle;
            EditorStyles.toggle.fontStyle = toggleFontStyle;
        }
    }
}