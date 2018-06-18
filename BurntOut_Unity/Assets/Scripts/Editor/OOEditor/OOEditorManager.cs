using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor.Internal
{
    /// <summary>
    /// Allows GUIElements to work together and draw correctly.
    /// </summary>
    public static class OOEditorManager
    {
        // Default horizontal spacing between queue items emptied into a horizontal rect
        private const int SPACING = 3;
        /// <summary>
        /// Set to true if you want to draw elements at once. 
        /// Particularly useful when drawing into a horizontal rect for a ReorderableList.
        /// </summary>
        public static bool Wait { get; set; }

        public static bool InHorizontal { get; set; }
        public static bool InToolbar { get; set; }

        /// <summary>
        /// Style used for labels and elements like foldouts. 
        /// 
        /// <para>Set with the <see cref="OOEditor.OverrideLabelStyle"/> class.</para>
        /// </summary>
        public static EditorStyle OverrideLabelStyle { get; set; }
        /// <summary>
        /// Style used for all text. Overrides all other styles.
        /// 
        /// <para>Set with the <see cref="OOEditor.OverrideTextStyle"/> class.</para>
        /// </summary>
        public static EditorStyle OverrideTextStyle { get; set; }

        /// <summary>
        /// Allows listening to whether there are changes in objects in a range
        /// </summary>
        public static event EventHandler Changed;
        public static void ElementChanged(object sender, EventArgs e)
        {
            Changed?.Invoke(sender, e);
        }

        // Queue of draw information
        // Should later use a struct or class, rather than a tuple
        private static readonly Queue<Tuple<EditorGUIElement, Action<Rect>, GUIContent>> drawQueue
            = new Queue<Tuple<EditorGUIElement, Action<Rect>, GUIContent>>();
        private static float minDrawWidth;
        /// <summary>
        /// Adds a GUI element to the draw queue. 
        /// Immediately empties the draw queue if <see cref="Wait"/> is false. 
        /// </summary>
        /// <param name="element">GUI element to draw</param>
        /// <param name="draw">Action to draw the element in a given rectangle</param>
        /// <param name="content">GUIContent to base element's height off of</param>
        /// <remarks>
        /// Element and content should later be replaced with a general width and height element.
        /// </remarks>
        internal static void DrawGuiElement(EditorGUIElement element, Action<Rect> draw, GUIContent content = null)
        {
            // If there's no content, create content with a single space to avoid any potential errors
            if (content == null)
                content = new GUIContent(" ");

            // Ensure enough room is reserved when drawing elements in horizontal rect
            if (element.MinWidth > 0)
                minDrawWidth += element.MinWidth;
            if (drawQueue.Count > 0)
                minDrawWidth += SPACING;

            // Organize the data together into a tuple (later may be its own struct)
            var tuple = new Tuple<EditorGUIElement, Action<Rect>, GUIContent>(element, draw, content);
            drawQueue.Enqueue(tuple);

            if (!Wait)
                EmptyQueue();
        }


        private static Rect horizontalRect;
        /// <summary>
        /// Resets the rectangle being used in width calculations.
        /// Automatically reset when not in a horizontal.
        /// </summary>
        public static void ResetHorizontalRect()
        {
            using (Horizontal.Draw())
                GUILayout.FlexibleSpace();
            horizontalRect = GUILayoutUtility.GetLastRect();
        }


        private static Rect queueRect;
        /// <summary>
        /// Draws all elements in the queue in a passed horizontal rectangle. 
        /// Particularly useful when drawing into a horizontal rect for a ReorderableList.
        /// </summary>
        /// <param name="rect">Rectangle to draw elements horizontally within</param>
        public static void EmptyQueueInHorizontalRect(Rect rect)
        {
            Wait = false;
            while (drawQueue.Count > 0)
            {
                var draw = drawQueue.Dequeue();
                var elem = draw.Item1;
                var displayAction = draw.Item2;

                Rect pos = GetPosInHorizontalRect(rect, elem);
                displayAction(pos);
                // Decrease the last position from the rectangle being drawn in
                rect.x += pos.width + SPACING;
                rect.width -= pos.width + SPACING;
            }
            minDrawWidth = 0;
        }

        // Calculates a position for an element in a horizontal rectangle.
        private static Rect GetPosInHorizontalRect(Rect rect, EditorGUIElement guiElement)
        {
            // Remove the calculations pertaining to this element from minDrawWidth
            minDrawWidth -= SPACING;
            if (guiElement.MinWidth > 0)
                minDrawWidth -= guiElement.MinWidth;

            // Our width is maxWidth if able. If there's no maxWidth, we just use an average width between elements
            // Ideally this should be much more complex, but this is good enough for now
            float maxWidth = rect.width - minDrawWidth;
            float preferredWidth = Mathf.Min(maxWidth, rect.width / (drawQueue.Count + 1));
            if (guiElement.MinWidth > 0)
                preferredWidth = guiElement.MinWidth;
            // Use the default width if possible
            if (guiElement.Width > 0 && guiElement.Width < maxWidth)
                preferredWidth = guiElement.Width;
            // Use the max width if possible and it is smaller than the preferred width
            if (guiElement.MaxWidth > 0 && guiElement.MaxWidth < preferredWidth)
                preferredWidth = guiElement.MaxWidth;

            rect.width = preferredWidth;

            return rect;
        }

        /// <summary>
        /// Draws all elements in the draw queue.
        /// </summary>
        public static void EmptyQueue()
        {
            Wait = false;
            while (drawQueue.Count > 0)
            {
                var draw = drawQueue.Dequeue();
                var elem = draw.Item1;
                var displayAction = draw.Item2;
                var contents = draw.Item3;
                Rect pos = GetDimensions(contents, elem);
                displayAction(pos);
            }
            minDrawWidth = 0;
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

            // The 8 pixels is the offset relevant for text areas. As this is only relevant to text areas right now, this should later be outsourced to them
            var height = style.CalcHeight(content, scale.width - 8);

            // I have no idea how to get height to work properly, so I just did all 3 of these to be safe.
            options.Add(GUILayout.MinHeight(height));
            options.Add(GUILayout.MaxHeight(height));
            options.Add(GUILayout.Height(height));

            Rect position = EditorGUILayout.GetControlRect(false, height, style, options.ToArray());
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
        /// <summary>
        /// Scales the passed width by the font-size.
        /// Default widths are based on a font-size of 11.
        /// </summary>
        /// <param name="width">Default width at font-size 11</param>
        /// <param name="fontSize">Font-size to scale to fit</param>
        /// <returns>A float of the width for the new font-size</returns>
        public static float ScaledWidth(float width, int fontSize)
        {
            if (fontSize == 0 || fontSize == 11)
                return width;
            else
                return (width / 11f) * widthScalars[fontSize - 1];
        }
    }
}
