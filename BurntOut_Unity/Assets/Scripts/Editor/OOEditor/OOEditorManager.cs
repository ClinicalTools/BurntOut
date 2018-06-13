using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor.Internal
{
    internal static class OOEditorManager
    {
        private const int SPACING = 3;

        private static bool wait;
        public static bool Wait
        {
            get
            {
                return wait;
            }
            set
            {
                //if (wait == true && value == false)
                    //EmptyQueue();
                wait = value;
            }
        }


        internal static int InToolbar { get; set; }
        internal static int InHorizontal { get; set; }
        internal static OverrideLabelStyle OverrideLabelStyle { get; set; }
        internal static OverrideTextStyle OverrideTextStyle { get; set; }

        private static Queue<GUIElement> drawElements = new Queue<GUIElement>();
        private static Queue<GUIContent> drawContents = new Queue<GUIContent>();
        private static Queue<Action<Rect>> draws = new Queue<Action<Rect>>();
        private static float minDrawWidth;
        internal static void DrawGuiElement(GUIElement element, Action<Rect> draw, GUIContent content = null)
        {
            if (content == null)
                content = new GUIContent(" ");

            if (element.MinWidth > 0)
                minDrawWidth += element.MinWidth;
            if (draws.Count > 0)
                minDrawWidth += SPACING;
            drawElements.Enqueue(element);
            drawContents.Enqueue(content);
            draws.Enqueue(draw);

            if (!Wait)
                EmptyQueue();
        }
        
        private static Rect queueRect;
        public static void EmptyQueueInHorizontalRect(Rect rect)
        {
            Wait = false;
            while (drawElements.Count > 0)
            {
                var draw = draws.Dequeue();
                var elem = drawElements.Dequeue();
                drawContents.Dequeue();
                Rect pos = GetPosInHorizontalRect(rect, elem);
                draw(pos);
                rect.x += pos.width + SPACING;
                rect.width -= pos.width + SPACING;
            }
            minDrawWidth = 0;
        }

        public static void EmptyQueue()
        {
            Wait = false;
            while (drawElements.Count > 0)
            {
                var draw = draws.Dequeue();
                var elem = drawElements.Dequeue();
                var contents = drawContents.Dequeue();
                Rect pos = GetDimensions(contents, elem);
                draw(pos);
            }
            minDrawWidth = 0;
        }

        private static Rect horizontalRect;
        internal static void ResetHorizontalRect()
        {
            using (new Horizontal())
                GUILayout.FlexibleSpace();
            horizontalRect = GUILayoutUtility.GetLastRect();
        }
        
        private static Rect GetPosInHorizontalRect(Rect rect, GUIElement guiElement)
        {
            if (guiElement.MinWidth > 0)
                minDrawWidth -= guiElement.MinWidth;
            float maxWidth = rect.width - minDrawWidth;
            float preferredWidth = Mathf.Min(maxWidth, rect.width / (draws.Count + 1));
            if (guiElement.MinWidth > 0)
                preferredWidth = guiElement.MinWidth;
            if (guiElement.Width > 0 && guiElement.Width < maxWidth)
                preferredWidth = guiElement.Width;
            if (guiElement.MaxWidth > 0 && guiElement.MaxWidth < preferredWidth)
                preferredWidth = guiElement.MaxWidth;

            rect.width = preferredWidth;

            return rect;
        }
        // Gets the rect for the GUI object based on the content, style, and width parameters
        private static Rect GetDimensions(GUIContent content, GUIElement guiElement)
        {
            GUIStyle style = guiElement.GUIStyle;

            // Add all the width options
            List<GUILayoutOption> options = new List<GUILayoutOption>();
            if (guiElement.MinWidth > 0)
                options.Add(GUILayout.MinWidth(guiElement.MinWidth));
            if (guiElement.MaxWidth > 0)
                options.Add(GUILayout.MaxWidth(guiElement.MaxWidth));
            if (guiElement.Width > 0)
                options.Add(GUILayout.Width(guiElement.Width));

            // Elements in toolbar are scaled to fit their contents
            if (guiElement.Width <= 0 && guiElement.MinWidth <= 0 && guiElement.MaxWidth <= 0 && InToolbar > 0)
                options.Add(GUILayout.Width(style.CalcSize(content).x));

            // This gets the width of the element without adding much (if any) height
            Rect scale;
            if (InHorizontal == 0)
                ResetHorizontalRect();

            scale = horizontalRect;

            var height = style.CalcHeight(content, scale.width - 8);

            // I have no idea how to get height to work properly, so I just did all 3 of these to be safe.
            options.Add(GUILayout.MinHeight(height));
            options.Add(GUILayout.MaxHeight(height));
            options.Add(GUILayout.Height(height));

            Rect position;
            position = EditorGUILayout.GetControlRect(false, height, style, options.ToArray());
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
    }
}
