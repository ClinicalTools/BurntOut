namespace OOEditor
{
    /// <summary>
    /// Creates an empty line.
    /// </summary>
    public static class BlankLine
    {
        private static readonly LabelField label = new LabelField(" ");
        /// <summary>
        /// Draws a blank line.
        /// </summary>
        public static void Draw()
        {
            label.Draw();
        }
    }
}