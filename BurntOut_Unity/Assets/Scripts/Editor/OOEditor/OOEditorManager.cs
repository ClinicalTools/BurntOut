using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor.Internal
{
    public static class OOEditorManager
    {
        private const int SPACING = 3;
        public static bool Wait { get; set; }

        public static bool InHorizontal { get; set; }
        public static bool InToolbar { get; set; }

        public static EditorStyle OverrideLabelStyle { get; set; }
        public static EditorStyle OverrideTextStyle { get; set; }

        /// <summary>
        /// Allows listening to whether there are changes in objects in a range
        /// </summary>
        public static event EventHandler Changed;
        public static void ElementChanged(object sender, EventArgs e)
        {
            Changed?.Invoke(sender, e);
        }

        private static Queue<EditorGUIElement> drawElements = new Queue<EditorGUIElement>();
        private static Queue<GUIContent> drawContents = new Queue<GUIContent>();
        private static Queue<Action<Rect>> draws = new Queue<Action<Rect>>();
        private static float minDrawWidth;
        internal static void DrawGuiElement(EditorGUIElement element, Action<Rect> draw, GUIContent content = null)
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
            using (Horizontal.Draw())
                GUILayout.FlexibleSpace();
            horizontalRect = GUILayoutUtility.GetLastRect();
        }

        private static Rect GetPosInHorizontalRect(Rect rect, EditorGUIElement guiElement)
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
        private static Rect GetDimensions(GUIContent content, EditorGUIElement guiElement)
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

            // This gets the width of the element without adding much (if any) height
            Rect scale;
            if (!InHorizontal)
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
        public static Rect FieldLabel(Rect position, GUIContent content, float minWidth)
        {
            // Typically use labelWidth for the width, but ensure at least 10 pixels are saved for the field
            var width = Mathf.Min(EditorGUIUtility.labelWidth, position.width - minWidth);
            Rect labelRect = new Rect(position.x, position.y, width, position.height);

            EditorGUI.LabelField(labelRect, content);
            position.x += width;
            position.width -= width;

            return position;
        }

        // Used to scale widths by the fontsize
        // Based on averages from multiple paragraphs of english text. 
        // Hopefully provides good average scales 
        private static readonly float[] widthScalars =
        {
             1.08f,  2.02f,  3.06f,  3.78f,  4.88f,  6.11f,  6.54f,  7.85f,  9.52f, 10.75f,
            11.00f, 12.09f, 12.82f, 13.59f, 14.27f, 15.99f, 16.71f, 17.39f, 18.65f, 19.51f,
            20.34f, 21.42f, 22.42f, 22.97f, 24.35f, 25.03f, 26.37f, 27.71f, 28.11f, 29.66f,
            30.19f, 31.08f, 32.38f, 33.07f, 33.86f, 34.89f, 35.82f, 36.84f, 38.19f, 39.1f,
            39.92f, 41.10f, 41.80f, 42.52f, 43.64f, 44.58f, 45.99f, 46.67f, 47.56f, 49.13f,
            49.86f, 50.55f, 52.03f, 52.51f, 53.46f, 54.35f, 55.36f, 56.17f, 57.14f, 58.26f,
            59.48f, 60.48f, 61.47f, 62.45f, 63.43f, 64.06f, 65.24f, 66.04f, 67.00f, 68.13f,
            68.94f, 70.35f, 71.14f, 72.18f, 72.94f, 73.99f, 74.9f, 75.58f, 76.76f, 77.88f,
            78.25f, 79.79f, 80.91f, 82.02f, 83.04f, 83.65f, 84.68f, 85.7f, 86.17f, 87.6f,
            88.45f, 88.94f, 90.41f, 91.15f, 92.51f, 93.7f, 94.21f, 95.11f, 96.46f, 97.05f
        };
        public static float ScaledWidth(float width, int fontSize)
        {
            if (fontSize == 0 || fontSize == 11)
                return width;
            else
                return (width / 11f) * widthScalars[fontSize - 1];
        }
    }
}
