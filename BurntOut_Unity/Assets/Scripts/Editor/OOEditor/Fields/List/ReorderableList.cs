using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace OOEditor
{
    public class ReorderableList<T, TDrawer> where TDrawer : GUIObjectDrawer<T>, new()
    {
        private List<TDrawer> drawers = new List<TDrawer>();
        public List<T> Value { get; private set; }
        private UnityEditorInternal.ReorderableList list;

        public ReorderableList(List<T> value)
        {
            Value = value;
            foreach (var val in Value)
            {
                var drawer = new TDrawer();
                drawer.Init(val);
                drawers.Add(drawer);
            }

            list = new UnityEditorInternal.ReorderableList(value, typeof(Task));
            list.headerHeight = 4;
            list.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    drawers[index].Draw();
                };
        }
        
        protected void Draw()
        {
            list.elementHeight = EditorStyles.popup.CalcHeight(new GUIContent(" "), 100) + 4;
            using (new Vertical())
                list.DoLayoutList();

            for (int i = 0; i < Value.Count; i++)
                Value[i] = drawers[i].Value;
        }
    }
}