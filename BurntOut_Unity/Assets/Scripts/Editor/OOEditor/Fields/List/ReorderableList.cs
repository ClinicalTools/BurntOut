using OOEditor.Internal;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace OOEditor
{
    public class ReorderableList<T, TDrawer> where TDrawer : GUIObjectDrawer<T>
    {
        private List<TDrawer> drawers = new List<TDrawer>();
        public List<T> Value { get; private set; }
        private ReorderableList list;

        public ReorderableList(List<T> value)
        {
            Value = value;
            foreach (var val in Value)
            {
                object[] args = { val };
                var drawer = (TDrawer)Activator.CreateInstance(typeof(TDrawer), args);
                drawers.Add(drawer);
            }

            list = new ReorderableList(Value, typeof(T))
            {
                headerHeight = 4,
                drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 1;
                    rect.height -= 4;
                    OOEditorManager.Wait = true;
                    drawers[index].Draw();
                    OOEditorManager.EmptyQueueInHorizontalRect(rect);
                },
                onAddCallback = (ReorderableList list) =>
                {
                    var val = (T)Activator.CreateInstance(typeof(T));
                    Value.Add(val);
                    object[] args = { val };
                    var drawer = (TDrawer)Activator.CreateInstance(typeof(TDrawer), args);
                    drawers.Add(drawer);
                },
                onRemoveCallback = (ReorderableList list) =>
                {
                    drawers.RemoveAt(list.index);
                    Value.RemoveAt(list.index);
                    list.index = -1;
                },
                onReorderCallback = (ReorderableList list) =>
                {
                    for (var i = 0; i < Value.Count; i++)
                        drawers[i].Value = Value[i];
                }
            };
        }

        public void Draw()
        {
            list.elementHeight = EditorStyles.popup.CalcHeight(new GUIContent(" "), 100) + 4;

            using (new Vertical())
                list.DoLayoutList();

            if (typeof(T).IsValueType)
                for (int i = 0; i < Value.Count; i++)
                    Value[i] = drawers[i].Value;
        }
    }
}