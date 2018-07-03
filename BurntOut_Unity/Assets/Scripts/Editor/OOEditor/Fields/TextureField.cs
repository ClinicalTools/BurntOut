using System;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Control to edit a line of text.
    /// </summary>
    public class SpriteField : GUIControlField<Sprite>
    {
        protected override GUIStyle BaseStyle =>
            new GUIStyle(EditorStyles.textField)
            {
                fixedHeight = ImageHeight
            };

        protected override float ReservedWidth => ImageWidth;

        /// <summary>
        /// Width of the sprite field
        /// </summary>
        public override float Width => ImageWidth;

        public float ImageHeight { get; set; } = 80;
        private float ImageWidth
        {
            get
            {
                if (Value == null || Value?.texture == null)
                    return ImageHeight;
                else
                    return Value.texture.width * (ImageHeight / Value.texture.height);
            }
        }
        /// <summary>
        /// Makes a text field for entering integers.
        /// </summary>
        /// <param name="value">The initial value being edited.</param>
        /// <param name="text">Optional label in front of the field.</param>
        /// <param name="tooltip">Tooltip of the optional label in front of the field.</param>
        /// <param name="image">Image to display at the front of the optional label.</param>
        public SpriteField(Sprite value, string text = null, string tooltip = null,
            Texture image = null) : base(text, tooltip, image)
        {
            Value = value;
        }

        protected override void Display(Rect position)
        {
            position.height = ImageHeight;

            if (position.width > ImageWidth)
                position.width = ImageWidth;

            Value = (Sprite)EditorGUI.ObjectField(position, Value, typeof(Sprite), false);
        }
    }
}