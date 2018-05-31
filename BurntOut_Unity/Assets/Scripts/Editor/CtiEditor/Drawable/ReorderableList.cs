using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CtiEditor.Drawable
{
    public class ReorderableList<T> : IEditorDrawable
    {
        UnityEditorInternal.ReorderableList list;
        internal static Rect DrawPosition;

        public ReorderableList(List<T> elements, Action<int> action, Action<Rect, int, bool, bool> action2)
        {
            list = new UnityEditorInternal.ReorderableList(elements, typeof(T));

            list.drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    CtiEditorGUI.InReorderableList = true;

                    var element = (Task)list.list[index];
                    rect.y += 1;
                    rect.height -= 4;

                    //CtiEditorGUI.InHorizontal++;
                    //using (CtiEditorGUI.Horizontal())
                    //action(index);

                    action2(rect, index, isActive, isFocused);

                    Rect newRect = new Rect(rect.x, rect.y, rect.width, rect.height);
                    //element.dialogue = EditorGUI.TextField(newRect, element.dialogue);
                    //CtiEditorGUI.InHorizontal--;

                    CtiEditorGUI.InReorderableList = false;
                };
        }

        public void Draw()
        {
            EditorStyles.popup.fixedHeight = 0;
            list.elementHeight = EditorStyles.popup.CalcHeight(new GUIContent(" "), 100) + 4;
            list.headerHeight = 4;
            using (CtiEditorGUI.Vertical())
                list.DoLayoutList();
        }
    }
}