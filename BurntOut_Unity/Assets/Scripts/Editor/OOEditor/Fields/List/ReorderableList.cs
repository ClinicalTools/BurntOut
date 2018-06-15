using OOEditor.Internal;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace OOEditor
{
    public class ReorderableList<T, TDrawer> : OOList<T, TDrawer>
        where TDrawer : IGUIObjectDrawer<T>
    {
        private ReorderableList list;

        public ReorderableList(List<T> value) : base(value)
        {
            list = new ReorderableList(Value, typeof(T))
            {
                headerHeight = 4,
                drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    rect.y += 1;
                    rect.height -= 4;
                    OOEditorManager.Wait = true;
                    Drawers[index].Draw();
                    OOEditorManager.EmptyQueueInHorizontalRect(rect);
                },
                onAddCallback = (ReorderableList list) =>
                {
                    AddRow();
                },
                onRemoveCallback = (ReorderableList list) =>
                {
                    RemoveRow(list.index);
                    list.index = -1;
                },
                onReorderCallback = (ReorderableList list) =>
                {
                    for (var i = 0; i < Value.Count; i++)
                        Drawers[i].Value = Value[i];
                }
            };
        }

        protected override void Display()
        {
            var style = new GUIStyle(EditorStyles.popup)
            {
                fixedHeight = 0
            };
            if (OOEditorManager.OverrideLabelStyle != null)
                OOEditorManager.OverrideLabelStyle.ApplyToStyle(style);
            if (OOEditorManager.OverrideTextStyle != null)
                OOEditorManager.OverrideTextStyle.ApplyToStyle(style);

            list.elementHeight = style.CalcHeight(new GUIContent(), 100) + 4;

            using (Vertical.Draw())
                list.DoLayoutList();
        }
    }
}