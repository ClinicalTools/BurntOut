using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    internal static class OOEditorManager
    {
        internal static bool wait;
        
        internal static int InToolbar { get; set; }
        internal static int InHorizontal { get; set; }
        internal static OverrideLabelStyle OverrideLabelStyle { get; set; }
        internal static OverrideTextStyle OverrideTextStyle { get; set; }

        private static Queue<GUIElement> drawElements = new Queue<GUIElement>();
        private static Queue<GUIContent> drawContents = new Queue<GUIContent>();
        private static Queue<Action<Rect>> draws = new Queue<Action<Rect>>();
        internal static void DrawGuiElement(GUIElement element, Action<Rect> draw, GUIContent content = null)
        {
            if (content == null)
                content = new GUIContent(" ");

            drawElements.Enqueue(element);
            drawContents.Enqueue(content);
            draws.Enqueue(draw);

            if (!wait)
                EmptyQueue();
        }

        private static void EmptyQueue()
        {
            while (drawElements.Count > 0)
            {
                var draw = draws.Dequeue();
                var elem = drawElements.Dequeue();
                var contents = drawContents.Dequeue();
                draw(GetDimensions(contents, elem));
            }
        }

        private static Rect horizontalRect;
        internal static void ResetHorizontalRect()
        {
            using (new Horizontal())
                GUILayout.FlexibleSpace();
            horizontalRect = GUILayoutUtility.GetLastRect();
        }
        // Gets the rect for the GUI object based on the content, style, and width parameters
        private static Rect GetDimensions(GUIContent content, GUIElement guiElement)
        {
            // Add all the width options
            List<GUILayoutOption> options = new List<GUILayoutOption>();
            if (guiElement.Width > 0)
                options.Add(GUILayout.Width(guiElement.Width));
            if (guiElement.MinWidth > 0)
                options.Add(GUILayout.MinWidth(guiElement.MinWidth));
            if (guiElement.MaxWidth > 0)
                options.Add(GUILayout.MaxWidth(guiElement.MaxWidth));

            // This gets the width of the element without adding much (if any) height
            Rect scale;
            if (InHorizontal == 0)
                ResetHorizontalRect();

            scale = horizontalRect;

            var height = guiElement.BaseStyle.CalcHeight(content, scale.width - 8);

            // I have no idea how to get height to work properly, so I just did all 3 of these to be safe.
            options.Add(GUILayout.MinHeight(height));
            options.Add(GUILayout.MaxHeight(height));
            options.Add(GUILayout.Height(height));

            Rect position;
            position = EditorGUILayout.GetControlRect(false, height, guiElement.BaseStyle, options.ToArray());
            position.height = height;

            return position;
        }

        // Creates a label for a field and returns the position for the field
        internal static Rect FieldLabel(Rect position, GUIContent content, float minWidth)
        {
            // Typically use labelWidth for the width, but ensure at least 10 pixels are saved for the field
            var width = Mathf.Min(EditorGUIUtility.labelWidth, position.width - minWidth);
            Rect labelRect = new Rect(position.x, position.y, width, position.height);

            EditorGUI.LabelField(labelRect, content);
            position.x += width;
            position.width -= width;

            return position;
        }

        internal static GUIStyle GetLabelStyle(EditorStyle labelStyle)
        {
            GUIStyle style = new GUIStyle(EditorStyles.label)
            {
                wordWrap = true,
                fixedHeight = 0
            };

            ApplyStyle(style, labelStyle);
            ApplyStyle(style, OverrideLabelStyle);
            ApplyStyle(style, OverrideTextStyle);

            return style;
        }

        internal static GUIStyle GetStyle(GUIElement element)
        {
            GUIStyle style = new GUIStyle(element.BaseStyle)
            {
                wordWrap = true,
                fixedHeight = 0
            };

            ApplyStyle(style, element.Style);
            ApplyStyle(style, OverrideLabelStyle);
            ApplyStyle(style, OverrideTextStyle);

            return style;
        }

        private static void ApplyStyle(GUIStyle style, EditorStyle editorStyle)
        {
            if (editorStyle == null)
                return;

            if (editorStyle.FontSize > 0)
                style.fontSize = editorStyle.FontSize;
            if (editorStyle.FontStyle != null)
                style.fontStyle = (FontStyle) editorStyle.FontStyle;
        }
    }
}
