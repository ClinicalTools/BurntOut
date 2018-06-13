using System;
using System.Collections.Generic;

namespace OOEditor
{
    public class GUIList<T, TDrawer> where TDrawer : IGUIObjectDrawer<T>
    {
        public List<T> Value { get; private set; }
        private List<TDrawer> drawers = new List<TDrawer>();

        private LabelField topUpSpace;
        private List<Button> upButtons = new List<Button>();
        private List<Button> downButtons = new List<Button>();
        private LabelField bottomDownSpace;
        private List<Button> delButtons = new List<Button>();
        private Button addButton;

        /// <summary>
        /// Used if the element to initialize shouldn't be initialized with the default constructor.
        /// </summary>
        public Func<T> DefaultElement { get; set; }

        public GUIList(List<T> value)
        {
            Value = value;
            for (var i = 0; i < Value.Count; i++)
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
                Value.Add(val);

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
                drawers.RemoveAt(index);
                delButtons.RemoveAt(Value.Count);
                if (Value.Count > 0)
                {
                    upButtons.RemoveAt(Value.Count - 1);
                    downButtons.RemoveAt(Value.Count - 1);
                }
            };
            delButtons.Add(delButton);
        }

        public void Draw()
        {
            while (drawers.Count < Value.Count)
                AddRow();

            for (var i = 0; i < Value.Count; i++)
            {
                using (new Horizontal())
                {
                    drawers[i].Draw();

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
            }

            addButton.Draw();

            if (typeof(T).IsValueType)
                for (int i = 0; i < Value.Count; i++)
                    Value[i] = drawers[i].Value;
        }
    }
}