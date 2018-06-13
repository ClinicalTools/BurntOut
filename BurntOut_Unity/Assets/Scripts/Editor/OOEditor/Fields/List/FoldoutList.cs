using CtiEditor;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace OOEditor
{
    public class FoldoutList<T, TDrawer> where TDrawer : FoldoutObjectDrawer<T>
    {
        public List<T> Value { get; set; }
        private List<TDrawer> drawers = new List<TDrawer>();

        private readonly LabelField topUpSpace;
        private readonly List<Button> upButtons = new List<Button>();
        private readonly List<Button> downButtons = new List<Button>();
        private readonly LabelField bottomDownSpace;
        private readonly List<Button> delButtons = new List<Button>();
        private readonly Button addButton;

        /// <summary>
        /// Used if the element to initialize shouldn't be initialized with the default constructor.
        /// </summary>
        public Func<T> DefaultElement { get; set; }

        public FoldoutList(List<T> value)
        {
            Value = value;
            for (var i = 0; i < value.Count; i++)
                AddRow();

            topUpSpace = new LabelField(" ")
            {
                Width = 40
            };
            bottomDownSpace = new LabelField(" ")
            {
                Width = 40
            };
            addButton = new Button("+")
            {
                Width = 128
            };
            addButton.Pressed += (object o, EventArgs e) =>
            {
                T val;
                if (DefaultElement != null)
                    val = DefaultElement();
                else
                    val = (T)Activator.CreateInstance(typeof(T));
                value.Add(val);

                AddRow();
            };
        }

        private void AddRow()
        {
            var index = drawers.Count;
            object[] args = { Value[index] };
            var drawer = (TDrawer)Activator.CreateInstance(typeof(TDrawer), args);
            drawers.Add(drawer);

            if (index > 0)
            {
                EventHandler swapPos = (object o, EventArgs e) =>
                {
                    var val = Value[index];
                    Value.RemoveAt(index);
                    Value.Insert(index - 1, val);
                    var draw = drawers[index];
                    drawers.RemoveAt(index);
                    drawers.Insert(index - 1, draw);
                };

                var upButton = new Button("▲")
                {
                    Width = 40
                };
                upButton.Pressed += swapPos;
                upButtons.Add(upButton);

                // Create the down button for the last row
                var downButton = new Button("▼")
                {
                    Width = 40
                };
                downButton.Pressed += swapPos;
                downButtons.Add(downButton);
            }

            var delButton = new Button("X")
            {
                Width = 40
            };
            delButton.Pressed += (object o, EventArgs e) =>
            {
                Value.RemoveAt(index);
                RemoveRow(index);
            };
            delButtons.Add(delButton);
        }

        private void RemoveRow(int index)
        {
            drawers.RemoveAt(index);
            delButtons.RemoveAt(Value.Count);
            if (Value.Count > 0)
            {
                upButtons.RemoveAt(Value.Count - 1);
                downButtons.RemoveAt(Value.Count - 1);
            }
        }

        // Since the list is shared outside this class, values must be constantly updated to avoid errors
        private void UpdateValues()
        {
            for (var i = 0; i < drawers.Count; i++)
                if (!drawers[i].Value.Equals(Value[i]))
                    drawers[i].Value = Value[i];

            while (drawers.Count < Value.Count)
                AddRow();
            while (drawers.Count > Value.Count)
                RemoveRow(drawers.Count - 1);
        }

        public void Draw()
        {
            UpdateValues();

            for (var i = 0; i < Value.Count; i++)
            {
                using (new Horizontal())
                {
                    drawers[i].DrawFoldout();

                    if (i > 0)
                        upButtons[i - 1].Draw();
                    else
                        topUpSpace.Draw();

                    if (i < Value.Count - 1)
                        downButtons[i].Draw();
                    else
                        bottomDownSpace.Draw();

                    delButtons[i].Draw();
                }

                if (drawers[i].Expanded)
                    using (new Indent())
                    using (new GUIContainer())
                        drawers[i].Draw();
            }

                addButton.Draw();

            // This allows value type lists to function correctly
            // Also ensures the drawer is using a valid reference
            for (int i = 0; i < Value.Count; i++)
                if (!drawers[i].Value.Equals(Value[i]))
                    Value[i] = drawers[i].Value;
        }
    }
}