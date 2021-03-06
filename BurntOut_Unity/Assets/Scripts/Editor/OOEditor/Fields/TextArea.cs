﻿using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Control to edit a potentially multiple-line block of text.
    /// </summary>
    public class TextArea : GUIControl<string>
    {
        /// <summary>
        /// Value being represented by the control.
        /// </summary>
        public override string Value
        {
            get { return base.Value; }
            set
            {
                base.Value = value ?? "";
                Content.text = Value;
            }
        }

        /// <summary>
        /// Content displayed by the control.
        /// </summary>
        public override GUIContent Content => new GUIContent(Value);
        
        protected override GUIStyle BaseStyle
        {
            get
            {
                EditorStyles.textArea.wordWrap = true;
                return EditorStyles.textArea;
            }
        }

        /// <summary>
        /// Makes a text area.
        /// </summary>
        /// <param name="value">Initial value to edit.</param>
        public TextArea(string value) : base(value)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            // Ensures value isn't null (happens on scene change for some reason)
            Value = Value;

            var style = GUIStyle;
            position.height = GUIStyle.CalcHeight(Content, ValidWidth);
            var spacing = position.height - style.CalcHeight(new GUIContent(" "), ValidWidth);
            // Spacing from new line characters is 
            var newLineCount = Value.Split('\n').Length - 1;
            // At font 20, lines start adding an extra 4 pixels, rather than 3. 
            // I doubt we'll go higher than that, so that's good enough for now.
            // 8 is the lowest I checked, and that also uses 3. No practical reason to go lower.
            spacing -= newLineCount * ((int)(style.fontSize * 1.055) + 3);
            GUILayoutUtility.GetRect(1, spacing);

            Value = EditorGUI.TextArea(position, Value, GUIStyle);
        }
    }
}