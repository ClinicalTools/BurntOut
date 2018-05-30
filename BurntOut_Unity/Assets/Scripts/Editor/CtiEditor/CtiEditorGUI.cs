using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CtiEditor
{
    /// <summary>
    /// Custom editor GUI that functions how I feel Unity's editor GUI classes (EditorGUI, EditorGUILayout and even GUILayout)
    /// should function.
    /// 
    /// FadeGroups, Horizontals, ScrollViews, ToggleGroups, Verticals, IndentLevel, and FontSize should use 
    /// their IDisposable equivalent.
    /// 
    /// GUILayoutOptions should not be used for setting heights.
    /// </summary>
    /// <remarks>
    /// I'm only implementing what I need. Feel free to expand on this class as needed. 
    /// Eventually it should hold all abilities in EditorGUI.
    /// 
    /// I also considered using the editor values by ref, but there's a few places where that is harder to implement (like values in arrays),
    /// so I left it out for consistency sake, especially since this class could potentially get huge.
    /// </remarks>
    public static class CtiEditorGUI
    {
        public static int InHorizontal
        {
            get; set;
        }
        private static Rect horizontalRect;

        /// <summary>
        /// Holds width data for a piece of the editor. 
        /// You don't use these directly, but construct them with the Width, MinWidth, and MaxWidth 
        /// functions in the CtiEditorGUI class.
        /// </summary>
        public class EditorGUIWidth
        {
            internal float width;
            internal float minWidth;
            internal float maxWidth;
        }

        /// <summary>
        /// Option passed to a control to give it an absolute width.
        /// </summary>
        public static EditorGUIWidth Width(float width)
        {
            return new EditorGUIWidth()
            {
                width = width
            };
        }
        /// <summary>
        /// Option passed to a control to specify a minimum width.
        /// </summary>
        /// <remarks>This option will override the Automatic width Layout parameter</remarks>
        public static EditorGUIWidth MinWidth(float minWidth)
        {
            return new EditorGUIWidth()
            {
                minWidth = minWidth
            };
        }
        /// <summary>
        /// Option passed to a control to specify a maximum width.
        /// </summary>
        public static EditorGUIWidth MaxWidth(float maxWidth)
        {
            return new EditorGUIWidth()
            {
                maxWidth = maxWidth
            };
        }

        #region INTERNAL
        // Gets the rect for the GUI object based on the content, style, and width parameters
        private static Rect GetDimensions(GUIContent content, GUIStyle style, params EditorGUIWidth[] widthOptions)
        {
            // Add all the width options
            List<GUILayoutOption> options = new List<GUILayoutOption>();
            foreach (var widthOption in widthOptions)
            {
                if (widthOption == null)
                    continue;
                if (widthOption.width > 0)
                    options.Add(GUILayout.Width(widthOption.width));
                if (widthOption.minWidth > 0)
                    options.Add(GUILayout.Width(widthOption.minWidth));
                if (widthOption.maxWidth > 0)
                    options.Add(GUILayout.Width(widthOption.maxWidth));
            }

            // This gets the width of the element without adding much (if any) height
            Rect scale;
            if (InHorizontal == 0)
            {
                using (Horizontal())
                    GUILayout.FlexibleSpace();
                scale = GUILayoutUtility.GetLastRect();
            }
            else
            {
                scale = horizontalRect;
            }
            var height = style.CalcHeight(content, scale.width - 8);

            // I have no idea how to get height to work properly, so i just did all 3 of these to be safe.
            options.Add(GUILayout.MinHeight(height));
            options.Add(GUILayout.MaxHeight(height));
            options.Add(GUILayout.Height(height));

            Rect position;
            position = EditorGUILayout.GetControlRect(false, height, style, options.ToArray());
            position.height = height;

            return position;
        }

        // Creates a label for a field and returns the position for the field
        private static Rect FieldLabel(Rect position, GUIContent content)
        {
            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            EditorGUI.LabelField(labelRect, content);
            position.x += EditorGUIUtility.labelWidth;
            position.width -= EditorGUIUtility.labelWidth;

            return position;
        }
        #endregion

        #region DISPOSABLES
        public static IDisposable Color(Color color)
        {
            return new Disposable.Color(color);
        }

        public static IDisposable Container()
        {
            return new Disposable.Vertical(EditorStyles.helpBox);
        }

        public static IDisposable FontStyle(FontStyle fontStyle)
        {
            return new Disposable.FontStyle(fontStyle);
        }

        public static IDisposable Horizontal()
        {
            if (InHorizontal == 0)
            {
                using (new Disposable.Horizontal())
                    GUILayout.FlexibleSpace();
                horizontalRect = GUILayoutUtility.GetLastRect();
            }

            return new Disposable.Horizontal();
        }

        public static IDisposable LabelFontStyle(FontStyle fontStyle)
        {
            return new Disposable.LabelFontStyle(fontStyle);
        }

        public static IDisposable Indent()
        {
            return new Disposable.Indent();
        }

        public static IDisposable FontSize(int fontSize)
        {
            return new Disposable.FontSize(fontSize);
        }

        public static IDisposable ScrollView(ref Vector2 scrollPosition)
        {
            return new Disposable.ScrollView(ref scrollPosition);
        }

        public static IDisposable Toolbar()
        {
            if (InHorizontal == 0)
            {
                using (new Disposable.Horizontal())
                    GUILayout.FlexibleSpace();
                horizontalRect = GUILayoutUtility.GetLastRect();
            }

            return new Disposable.Horizontal(EditorStyles.toolbar);
        }

        public static IDisposable Vertical()
        {
            return new Disposable.Vertical();
        }
        #endregion

        #region UI ELEMENTS
        /// <summary>
        /// Make a single press button. The user clicks them and something happens immediately.
        /// </summary>
        /// <param name="text">	Text to display on the button.</param>
        /// <param name="tooltip">Text to display on control hover.</param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        /// <returns>true when the users clicks the button</returns>
        public static bool Button(string text, string tooltip = null, params EditorGUIWidth[] widthOptions)
        {
            var style = EditorStyles.miniButton;

            var content = new GUIContent(text, tooltip);
            var position = GetDimensions(content, style, widthOptions);

            return GUI.Button(position, content, style);
        }

        /// <summary>
        ///     Make an enum popup selection field.
        ///     <para>
        ///         Takes the currently selected enum value as a parameter and returns 
        ///         the enum value selected by the user.
        ///     </para>
        /// </summary>
        /// <param name="value">The enum option the field shows.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">Text to display on control hover.</param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        /// <returns>The enum option that has been selected by the user.</returns>
        public static Enum EnumPopup(Enum value, string text = "", string tooltip = "", 
            params EditorGUIWidth[] widthOptions)
        {
            var style = EditorStyles.popup;
            style.fixedHeight = 0;

            var content = new GUIContent(text, tooltip);
            var position = GetDimensions(content, style, widthOptions);

            if (text != "")
                position = FieldLabel(position, content);

            return EditorGUI.EnumPopup(position, value, style);
        }

        /// <summary>
        ///     Make a label with a foldout arrow to the left of it.
        ///     <para>
        ///         This is useful for creating tree or folder like structures where 
        ///         child objects are only shown if the parent is folded out.
        ///     </para>
        /// </summary>
        /// <param name="value">The shown foldout state.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">Text to display on control hover.</param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        /// <returns>The foldout state selected by the user. If true, you should render sub-objects.</returns>
        public static bool Foldout(bool value, string text, string tooltip = "", 
            params EditorGUIWidth[] widthOptions)
        {
            var style = EditorStyles.foldout;
            var content = new GUIContent(text, tooltip);
            var position = GetDimensions(content, style, widthOptions);

            return EditorGUI.Foldout(position, value, content, style);
        }

        /// <summary>
        ///     Make a slider the user can drag to change an integer value between a min and a max.
        /// </summary>
        /// <param name="value">The value the slider shows. This determines the position of the draggable thumb.</param>
        /// <param name="min">The value at the left end of the slider.</param>
        /// <param name="max">The value at the right end of the slider.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">Text to display on control hover.</param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        /// <returns>The value that has been set by the user.</returns>
        public static int IntSlider(int value, int min, int max, string text = null, string tooltip = null,
            params EditorGUIWidth[] widthOptions)
        {
            GUIStyle style = EditorStyles.numberField;
            var content = new GUIContent(text, tooltip);
            var position = GetDimensions(content, style, widthOptions);

            if (text != "")
                position = FieldLabel(position, content);

            return EditorGUI.IntSlider(position, value, min, max);
        }

        /// <summary>
        /// Make a label field. (Useful for showing read-only info.)
        /// </summary>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">Text to display on control hover.</param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        public static void LabelField(string text, string tooltip = "", params EditorGUIWidth[] widthOptions)
        {
            var style = EditorStyles.label;
            var content = new GUIContent(text, tooltip);
            var position = GetDimensions(content, style, widthOptions);

            EditorGUI.LabelField(position, content, style);
        }

        /// <summary>
        ///     Make an object field. You can assign objects either by drag and drop objects 
        ///     or by selecting an object using the Object Picker.
        /// </summary>
        /// <typeparam name="T">The type of the objects that can be assigned.</typeparam>
        /// <param name="value">The object the field shows.</param>
        /// <param name="allowSceneObjects">Allow assigning scene objects.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">Text to display on control hover.</param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        /// <returns>The object that has been set by the user.</returns>
        public static UnityEngine.Object ObjectField<T>(UnityEngine.Object value, bool allowSceneObjects,
            string text = "", string tooltip = "", params EditorGUIWidth[] widthOptions)
        {
            var style = EditorStyles.objectField;
            var content = new GUIContent(text, tooltip);
            var position = GetDimensions(content, style, widthOptions);

            if (text != "")
                position = FieldLabel(position, content);

            return EditorGUI.ObjectField(position, value, typeof(T), allowSceneObjects);
        }

        /// <summary>
        ///     Make a generic popup selection field.
        ///     <para>
        ///         Takes the currently selected index as a parameter and returns the index selected by the user.
        ///     </para>
        /// </summary>
        /// <param name="value">The index of the option the field shows.</param>
        /// <param name="optionTexts">An array with the options shown in the popup.</param>
        /// <param name="optionToolTips">An array with the tooltips for the options shown in the popup.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">Text to display on control hover.</param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        /// <returns>The index of the option that has been selected by the user.</returns>
        public static int Popup(int value, string[] optionTexts, string[] optionToolTips = null,
            string text = "", string tooltip = "", params EditorGUIWidth[] widthOptions)
        {
            var style = EditorStyles.popup;
            var content = new GUIContent(text, tooltip);
            var position = GetDimensions(content, style, widthOptions);

            var displayedOptions = new GUIContent[optionTexts.Length];
            for (int i = 0; i < displayedOptions.Length; i++)
                if (optionToolTips != null && optionToolTips.Length > i)
                    displayedOptions[i] = new GUIContent(optionTexts[i], optionToolTips[i]);
                else
                    displayedOptions[i] = new GUIContent(optionTexts[i]);

            if (text != "")
                position = FieldLabel(position, content);

            return EditorGUI.Popup(position, value, displayedOptions, style);
        }

        /// <summary>
        /// Make a text area.
        /// </summary>
        /// <param name="value">The text to edit.</param>
        /// <param name="lastWidth">
        ///     Initialize OUTSIDE OF YOUR METHOD THAT CALLS THIS. 
        ///     This can be shared between TextArea's guaranteed to have the same width.
        ///     <para>
        ///         Unity gives errenous data every other frame, so this variable needs to be passed for 
        ///         things to work properly.
        ///     </para>
        /// </param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        /// <returns>The text entered by the user.</returns>
        public static string TextArea(string value, ref float lastWidth, params EditorGUIWidth[] widthOptions)
        {
            var style = EditorStyles.textArea;
            style.wordWrap = true;
            var content = new GUIContent(value);
            var position = GetDimensions(content, style, widthOptions);

            // This is literally the stupidest thing I've ever had to code
            // Screw Unity's editor system. It's so awful
            // Literally every other frame and the rectangles it gives you all have length and width 1
            // That frame happens to be the frame it decides to use my spacing
            if (position.width > 1)
                lastWidth = position.width;

            position.height = style.CalcHeight(content, lastWidth);
            var spacing = position.height - style.CalcHeight(new GUIContent(" "), lastWidth);
            var newLineCount = value.Split('\n').Length - 1;
            // At font 20, lines start adding an extra 4 pixels, rather than 3. 
            // I doubt we'll go higher than that, so that's good enough for now.
            // 8 is the lowest I checked, and that also uses 3. No practical reason to go lower.
            spacing -= newLineCount * ((int)(style.fontSize * 1.055) + 3);
            GUILayoutUtility.GetRect(1, spacing);

            return EditorGUI.TextArea(position, value, style);
        }

        /// <summary>
        /// Make a text field.
        /// </summary>
        /// <param name="value">The text to edit.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">Text to display on control hover.</param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        /// <returns>The text entered by the user.</returns>
        public static string TextField(string value, string text = "", string tooltip = "",
            params EditorGUIWidth[] widthOptions)
        {
            var style = EditorStyles.textField;
            var content = new GUIContent(text, tooltip);
            var position = GetDimensions(content, style, widthOptions);

            if (text != "")
                position = FieldLabel(position, content);

            return EditorGUI.TextField(position, value, style);
        }

        /// <summary>
        /// Make a toggle.
        /// </summary>
        /// <param name="value">The shown state of the toggle.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">Text to display on control hover.</param>
        /// <param name="widthOptions">
        ///     Options specifying default width values. 
        ///     Use Width(float), MaxWidth(float), MinWidth(float) to instantiate these options.
        /// </param>
        /// <returns>The selected state of the toggle.</returns>
        public static bool Toggle(bool value, string text = null, string tooltip = null, 
            params EditorGUIWidth[] widthOptions)
        {
            var style = EditorStyles.toggle;
            var content = new GUIContent(text, tooltip);
            var position = GetDimensions(content, style, widthOptions);


            if (text != "")
                position = FieldLabel(position, content);

            return EditorGUI.Toggle(position, value, style);
        }
        #endregion

    }
}