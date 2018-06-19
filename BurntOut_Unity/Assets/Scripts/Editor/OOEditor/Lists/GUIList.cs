using System;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// Basic editor list that displays a list of elements.
    /// 
    /// <para>Each list element will be drawn within a horizontal.</para>
    /// </summary>
    /// <typeparam name="T">Type of element to draw</typeparam>
    /// <typeparam name="TDrawer">Drawer type to use to draw each element</typeparam>
    public class GUIList<T, TDrawer> : OOList<T, TDrawer>
        where TDrawer : IGUIObjectDrawer<T>
    {
        // Width to draw up, down, and delete buttons at
        private const float BUTTON_WIDTH = 30;

        // Used in place of a button, when one shouldn't be drawn for some reason
        protected GUISpace ButtonSpace { get; }
        protected List<Button> UpButtons { get; } = new List<Button>();
        protected List<Button> DownButtons { get; } = new List<Button>();
        protected List<Button> DelButtons { get; } = new List<Button>();
        protected Button AddButton { get; }

        /// <summary>
        /// Creates a new GUIList to display the values in the passed list.
        /// </summary>
        /// <param name="value">List of values to display.</param>
        public GUIList(List<T> value) : base(value)
        {
            ButtonSpace = new GUISpace(BUTTON_WIDTH);

            AddButton = new Button("+")
            {
                MaxWidth = 128
            };
            AddButton.Style.FontStyle = FontStyle.Bold;
            AddButton.Pressed += (sender, e) =>
            {
                AddRow();
            };
        }

        // Manages the adding of a row and its associated buttons
        protected override void AddRow()
        {
            base.AddRow();

            // Since we're adding to the end of the current button lists, 
            //  the number of buttons in delete buttons is what the new index is
            var index = DelButtons.Count;

            if (index > 0)
            {
                // Swaps this row with the previous row
                EventHandler swapPos = (object o, EventArgs e) =>
                {
                    SwapRows(index, index - 1);
                };

                var upButton = new Button("▲", "Move up")
                {
                    MaxWidth = BUTTON_WIDTH
                };
                upButton.Pressed += swapPos;
                UpButtons.Add(upButton);

                // Create the down button for the last row, which conveniently 
                //  has the same functionality as the up button for this row
                var downButton = new Button("▼", "Move down")
                {
                    MaxWidth = BUTTON_WIDTH
                };
                downButton.Pressed += swapPos;
                DownButtons.Add(downButton);
            }

            var delButton = new Button("X", "Delete")
            {
                MaxWidth = BUTTON_WIDTH
            };
            delButton.Style.FontStyle = FontStyle.Bold;
            delButton.Pressed += (sender, e) =>
            {
                RemoveRow(index);
            };
            DelButtons.Add(delButton);
        }

        // Manages removing a row and its associated buttons
        protected override void RemoveRow(int index)
        {
            base.RemoveRow(index);

            DelButtons.RemoveAt(List.Count);
            if (List.Count > 0)
            {
                UpButtons.RemoveAt(List.Count - 1);
                DownButtons.RemoveAt(List.Count - 1);
            }
        }

        /// <summary>
        /// Draws the editor list.
        /// </summary>
        protected override void Display()
        {
            for (var i = 0; i < List.Count; i++)
            {
                using (Horizontal.Draw())
                {
                    Drawers[i].Draw(List[i]);

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
            }

            AddButton.Draw();
        }
    }
}