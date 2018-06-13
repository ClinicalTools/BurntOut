﻿using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class TextArea : GUIControl<string>
    {
        private string value = "";
        public override string Value
        {
            get { return value; }
            set
            {
                if (value == null)
                    this.value = "";
                else
                    this.value = value;
            }
        }

        protected override GUIStyle BaseStyle
        {
            get
            {
                EditorStyles.textArea.wordWrap = true;
                return EditorStyles.textArea;
            }
        }

        public TextArea(string value) : base(value)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            var style = GUIStyle;
            position.height = GUIStyle.CalcHeight(Content, ValidWidth);
            var spacing = position.height - style.CalcHeight(new GUIContent(" "), ValidWidth);
            var newLineCount = value.Split('\n').Length - 1;
            // At font 20, lines start adding an extra 4 pixels, rather than 3. 
            // I doubt we'll go higher than that, so that's good enough for now.
            // 8 is the lowest I checked, and that also uses 3. No practical reason to go lower.
            spacing -= newLineCount * ((int)(style.fontSize * 1.055) + 3);
            GUILayoutUtility.GetRect(1, spacing);

            Value = EditorGUI.TextArea(position, Value, GUIStyle);
            Content.text = Value;
        }
    }
}