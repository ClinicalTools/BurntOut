using System.Collections.Generic;

namespace OOEditor
{
    /// <summary>
    /// Editor list that contains a foldout for each element.
    /// </summary>
    /// <typeparam name="T">Type of element to draw</typeparam>
    /// <typeparam name="TDrawer">Drawer type to use to draw each element</typeparam>
    public class FoldoutList<T, TDrawer> : GUIList<T, TDrawer>
        where TDrawer : FoldoutClassDrawer<T>
    {
        /// <summary>
        /// Creates a new FoldoutList to display the values in the passed list.
        /// </summary>
        /// <param name="value">List of values to display</param>
        public FoldoutList(List<T> value) : base(value) { }
        
        /// <summary>
        /// Draws the foldout list.
        /// </summary>
        protected override void Display()
        {
            for (var i = 0; i < List.Count; i++)
            {
                using (Horizontal.Draw())
                {
                    Drawers[i].DrawFoldout();

                    // The first element cannot be moved up,
                    //  so a blank space is used instead of a button
                    if (i > 0)
                        UpButtons[i - 1].Draw();
                    else
                        ButtonSpace.Draw();

                    // The last element cannot be moved down,
                    //  so a blank space is used instead of a button
                    if (i < List.Count - 1)
                        DownButtons[i].Draw();
                    else
                        ButtonSpace.Draw();

                    DelButtons[i].Draw();
                }

                // Ensure the drawer is still valid, 
                //  in case it was deleted during the last draw call
                if (i < Drawers.Count && Drawers[i].Expanded)
                    using (Indent.Draw())
                    using (GUIContainer.Draw())
                        Drawers[i].Draw(List[i]);
            }

            AddButton.Draw();
        }
    }
}