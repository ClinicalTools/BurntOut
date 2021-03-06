﻿using OOEditor.Internal;
using System;
using System.Collections.Generic;
using UnityEngine;

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
        /// <param name="value">List of values to display.</param>
        /// <param name="reorderable">True if the elements can be reordered through displayed buttons.</param>
        /// <param name="removable">True if the elements can be removed through displayed buttons.</param>
        /// <param name="addable">True if new elements can be added through a displayed button.</param>
        public FoldoutList(List<T> value, bool reorderable = true, bool removable = true,
            bool addable = true) : base(value, reorderable, removable, addable) { }

        protected override void AddRow()
        {
            base.AddRow();

            // Buttons of the row should highlight the foldout that moved
            var index = UpButtons.Count - 1;
            if (index > -1)
            {
                UpButtons[index].Pressed += (sender, e) =>
                {
                    OOEditorManager.SetFocusedControl(Drawers[index].Foldout.Name);
                    //GUI.FocusControl(Drawers[index + 1].Foldout.Name);
                    //Debug.Log("a" + Drawers[index].Foldout.Content.text);
                };

                DownButtons[index].Pressed += (sender, e) =>
                {
                    OOEditorManager.SetFocusedControl(Drawers[index + 1].Foldout.Name);
                    //GUI.FocusControl(Drawers[index].Foldout.Name);
                    //Debug.Log("b" + Drawers[index].Foldout.Content.text);
                };
            }
        }

        /// <summary>
        /// Draws the foldout list.
        /// </summary>
        protected override void Display()
        {
            for (var i = 0; i < List.Count; i++)
            {
                Drawers[i].Index = i;

                using (Horizontal.Draw())
                {
                    Drawers[i].DrawFoldout();

                    if (Reorderable)
                    {
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
                    }

                    if (Removable)
                        DelButtons[i].Draw();
                }

                // Ensure the drawer is still valid, 
                //  in case it was deleted during the last draw call
                if (i < Drawers.Count && Drawers[i].Expanded)
                    using (Indent.Draw())
                    using (GUIContainer.Draw())
                        Drawers[i].Draw();
            }

            if (Addable)
                AddButton.Draw();
        }
    }
}