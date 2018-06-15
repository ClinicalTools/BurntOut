using System;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public class GUIList<T, TDrawer> : OOList<T, TDrawer>
        where TDrawer : IGUIObjectDrawer<T>
    {
        private const float BUTTON_WIDTH = 30;

        protected LabelField TopUpSpace { get; }
        protected List<Button> UpButtons { get; } = new List<Button>();
        protected List<Button> DownButtons { get; } = new List<Button>();
        protected LabelField BottomDownSpace { get; }
        protected List<Button> DelButtons { get; } = new List<Button>();
        protected Button AddButton { get; }

        public GUIList(List<T> value) : base(value)
        {
            TopUpSpace = new LabelField(" ")
            {

                MaxWidth = BUTTON_WIDTH
            };
            BottomDownSpace = new LabelField(" ")
            {
                MaxWidth = BUTTON_WIDTH
            };
            AddButton = new Button("+")
            {
                MaxWidth = 128
            };
            AddButton.Style.FontStyle = FontStyle.Bold;
            AddButton.Pressed += (object o, EventArgs e) =>
            {
                AddRow();
            };
        }

        protected override void AddRow()
        {
            base.AddRow();

            var index = DelButtons.Count;

            if (index > 0)
            {
                EventHandler swapPos = (object o, EventArgs e) =>
                {
                    SwapRows(index, index - 1);
                };

                var upButton = new Button("▲")
                {
                    MaxWidth = BUTTON_WIDTH
                };
                upButton.Pressed += swapPos;
                UpButtons.Add(upButton);

                // Create the down button for the last row
                var downButton = new Button("▼")
                {
                    MaxWidth = BUTTON_WIDTH
                };
                downButton.Pressed += swapPos;
                DownButtons.Add(downButton);
            }

            var delButton = new Button("X")
            {
                MaxWidth = BUTTON_WIDTH
            };
            delButton.Style.FontStyle = FontStyle.Bold;
            delButton.Pressed += (object o, EventArgs e) =>
            {
                RemoveRow(index);
            };
            DelButtons.Add(delButton);
        }

        protected override void RemoveRow(int index)
        {
            base.RemoveRow(index);

            DelButtons.RemoveAt(Value.Count);
            if (Value.Count > 0)
            {
                UpButtons.RemoveAt(Value.Count - 1);
                DownButtons.RemoveAt(Value.Count - 1);
            }
        }

        protected override void Display()
        {
            for (var i = 0; i < Value.Count; i++)
            {
                using (Horizontal.Draw())
                {
                    Drawers[i].Draw();

                    if (i > 0)
                        UpButtons[i - 1].Draw();
                    else
                        TopUpSpace.Draw();

                    if (i < Value.Count - 1)
                        DownButtons[i].Draw();
                    else
                        BottomDownSpace.Draw();

                    DelButtons[i].Draw();
                }
            }

            AddButton.Draw();
        }
    }
}