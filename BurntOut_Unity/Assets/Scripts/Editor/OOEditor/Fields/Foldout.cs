using OOEditor.Internal;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A label with a foldout arrow to the left of it.
    /// </summary>
    public class Foldout : GUIControl<bool>
    {
        protected override GUIStyle BaseStyle =>
            new GUIStyle(EditorStyles.foldout)
            {
                imagePosition = ImagePosition.ImageAbove
            };

        private GUIStyle guiLabelStyle;
        /// <summary>
        /// Resets the value of GUIStyle and guiLabelStyle.
        /// </summary>
        protected override void ResetGUIStyle()
        {
            // Reset GUIStyle
            base.ResetGUIStyle();

            guiLabelStyle = new GUIStyle(EditorStyles.label)
            {
                imagePosition = ImagePosition.ImageAbove
            };

            if (Focused && !OOEditorManager.InToolbar)
            {
                guiLabelStyle.normal = guiLabelStyle.focused;
                guiLabelStyle.onNormal = guiLabelStyle.onFocused;
            }

            Style.ApplyToStyle(guiLabelStyle);
            // Basic elements treated like labels
            if (OOEditorManager.OverrideLabelStyle != null)
                OOEditorManager.OverrideLabelStyle.ApplyToStyle(guiLabelStyle);
            if (OOEditorManager.OverrideTextStyle != null)
                OOEditorManager.OverrideTextStyle.ApplyToStyle(guiLabelStyle);
        }

        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">The tooltip of the foldout.</param>
        /// <param name="image">Image to draw at the beginning of the foldout.</param>
        public Foldout(string text = null, string tooltip = null, Texture image = null) 
            : base(text, tooltip, image) { }
        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="value">Whether the foldout starts expanded.</param>
        /// <summary>
        /// Makes a label with a foldout arrow to the left of it.
        /// </summary>
        /// <param name="value">Whether the foldout starts expanded.</param>
        /// <param name="text">The label to show.</param>
        /// <param name="tooltip">The tooltip of the foldout.</param>
        /// <param name="image">Image to draw at the beginning of the foldout.</param>
        public Foldout(bool value, string text = null, string tooltip = null, Texture image = null)
            : base(text, tooltip, image)
        {
            Value = value;
        }

        private const float FOLDOUT_WIDTH = 12;
        // Height of a foldout at the default font-size (11)
        private const float FOLDOUT_DEFAULT_HEIGHT = 16;
        protected override void Display(Rect position)
        {
            // This alligns the arrow to the center of the text (below the image)
            position.width -= FOLDOUT_WIDTH;
            var foldoutContent = new GUIContent(Content) 
            {
                image = null
            };
            var textHeight = GUIStyle.CalcHeight(foldoutContent, position.width);
            // I want it in the middle, so finding the height between the default and the new gets that
            var foldoutHeight = (textHeight + FOLDOUT_DEFAULT_HEIGHT) / 2;
            // Start position is measured from the bottom of the position rectangle
            var foldoutY = position.y + position.height - foldoutHeight;
            var foldoutPos = new Rect(position.x, foldoutY, FOLDOUT_WIDTH, foldoutHeight);
            
            Value = EditorGUI.Foldout(foldoutPos, Value, new GUIContent(), GUIStyle);

            // Move the label left of the foldout arrow
            position.x += FOLDOUT_WIDTH;
            // Gives the label a unique name that can be referenced by the element's name
            GUI.SetNextControlName(Name + "label");
            //GUIStyle.normal = GUIStyle.focused;
            //GUIStyle.onNormal = GUIStyle.onFocused;
            EditorGUI.LabelField(position, Content, guiLabelStyle);

            // If the label is clicked, focus it
            if (Event.current.rawType == EventType.MouseDown &&
                position.Contains(Event.current.mousePosition))
            {
                GUI.FocusControl(Name);

                // Needed for things to be selected correctly
                EditorWindow.focusedWindow.Repaint();
            }
        }
    }
}