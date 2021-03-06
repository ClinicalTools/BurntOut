﻿using OOEditor.Internal;
using UnityEditor;
using UnityEngine;
using System;

namespace OOEditor
{
    /// <summary>
    /// A GUI control that can have a preluding label.
    /// </summary>
    /// <typeparam name="T">Type of value to store</typeparam>
    public abstract class GUIControlField<T> : GUIControl<T>
    {
        /// <summary>
        /// Text style to draw the label with.
        /// </summary>
        public EditorStyle LabelStyle { get; } = new EditorStyle();

        // Width cannot be estimated when the element is using the content for a label
        public override float Width { get; } = 0;

        /// <summary>
        /// Internal use only. The style to draw the label in.
        /// </summary>
        public GUIStyle GUILabelStyle { get; protected set; }
        /// <summary>
        /// Resets the value of GUIStyle and GUILabelStyle.
        /// </summary>
        protected override void ResetGUIStyle()
        {
            // Reset GUIStyle
            base.ResetGUIStyle();
            
            GUILabelStyle = new GUIStyle(EditorStyles.label);
            if (Focused)
            {
                GUILabelStyle.normal = GUILabelStyle.focused;
                GUILabelStyle.onNormal = GUILabelStyle.onFocused;
            }

            if (OOEditorManager.OverrideLabelStyle != null)
                OOEditorManager.OverrideLabelStyle.ApplyToStyle(GUILabelStyle);
            if (OOEditorManager.OverrideTextStyle != null)
                OOEditorManager.OverrideTextStyle.ApplyToStyle(GUILabelStyle);

            // Apply the regular style, then override it with the label style.
            Style.ApplyToStyle(GUILabelStyle);
            LabelStyle.ApplyToStyle(GUILabelStyle);
        }

        /// <summary>
        /// Label portion will avoid using this much width if possible.
        /// </summary>
        protected abstract float ReservedWidth { get; }

        protected GUIControlField(string text = null, string tooltip = null, Texture image = null) 
            : base(text, tooltip, image) { }

        // Draws the label portion of the control
        protected override void PrepareDisplay(Rect position)
        {
            UpdateFocused();
            if (Content != null && !string.IsNullOrEmpty(Content.text))
            {
                // Ensure width is at least as big as the larger of the minimum widths
                float minWidth = Mathf.Max(ReservedWidth, MinWidth);

                // Typically use labelWidth for the width, but ensure at least MinWidth pixels are saved for the field
                var scaledLabelWidth = OOEditorManager.ScaledWidth(EditorGUIUtility.labelWidth, 
                    OOEditorManager.OverrideTextStyle?.FontSize ?? 0);
                var width = Mathf.Min(scaledLabelWidth, position.width - minWidth);
                // Width cannot be less than 0
                width = Mathf.Max(width, 1);
                Rect labelRect = new Rect(position.x, position.y, width, position.height);
                GUILabelStyle.imagePosition = ImagePosition.ImageAbove;
                // Gives the label a unique name that can be referenced by the element's name
                GUI.SetNextControlName(Name + "label");
                EditorGUI.LabelField(labelRect, Content, GUILabelStyle);

                // If the label is clicked, focus it
                if (Event.current.rawType == EventType.MouseDown &&
                    labelRect.Contains(Event.current.mousePosition))
                {
                    GUI.FocusControl(Name + "label");

                    // Needed for things to be selected correctly
                    EditorWindow.focusedWindow.Repaint();
                }

                // Remove the area the label was drawn in from the position rectangle
                position.x += width;
                position.width -= width;
            }
            else if (Content != null && !string.IsNullOrEmpty(Content.tooltip))
            {
                var tooltipContent = new GUIContent
                {
                    tooltip = Content.tooltip
                };
                EditorGUI.LabelField(position, tooltipContent);
            }

            base.PrepareDisplay(position);
        }
    }
}