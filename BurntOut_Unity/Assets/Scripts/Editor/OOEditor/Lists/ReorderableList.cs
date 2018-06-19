using OOEditor.Internal;
using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace OOEditor
{
    /// <summary>
    /// A list with elements that can easily be dragged up and down. 
    /// 
    /// <para>Each list element will effectively be drawn within a rectangle.</para>
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TDrawer"></typeparam>
    public class ReorderableList<T, TDrawer> : OOList<T, TDrawer>
        where TDrawer : IGUIObjectDrawer<T>
    {
        private ReorderableList list;

        /// <summary>
        /// Creates a new ReorderableList to display the values in the passed list.
        /// </summary>
        /// <param name="value">List of values to display.</param>
        public ReorderableList(List<T> value) : base(value)
        {
            list = new ReorderableList(List, typeof(T))
            {
                // Makes the header practically invisible
                headerHeight = 4,
                drawElementCallback =
                (Rect rect, int index, bool isActive, bool isFocused) =>
                {
                    // Ensures elements are well alligned in their row
                    rect.y += 1;
                    rect.height -= 4;

                    // Check if currently processing a mouse event
                    var curr = Event.current;
                    var mouseEvent = curr.button == 0 && curr.isMouse;


                    OOEditorManager.Wait = true;
                    Drawers[index].Draw(List[index]);
                    OOEditorManager.EmptyQueueInHorizontalRect(rect);

                    if (mouseEvent)
                    {
                        // Check if the mouse event was consumed
                        curr = Event.current;
                        mouseEvent = curr.button == 0 && curr.isMouse;

                        // An element within this row was clicked, so this is the active row
                        if (!mouseEvent)
                            list.index = index;
                    }
                },
                onAddCallback = (ReorderableList list) =>
                {
                    AddRow();
                },
                onRemoveCallback = (ReorderableList list) =>
                {
                    RemoveRow(list.index);
                    // No element should be selected after the selected element was removed
                    list.index = -1;
                },
                onReorderCallbackWithDetails = (ReorderableList list, int oldIndex, int newIndex) =>
                {
                    var drawer = Drawers[oldIndex];
                    Drawers.RemoveAt(oldIndex);
                    Drawers.Insert(newIndex, drawer);
                },
                // Remove the selection from the highlighted control when a row is clicked
                onSelectCallback = (ReorderableList list) =>
                {
                    OOEditorManager.ResetFocusedControl();
                }
            };
        }

        /// <summary>
        /// Draws the reorderable list.
        /// </summary>
        protected override void Display()
        {
            // Arbitrarily using popup as the style 
            var style = new GUIStyle(EditorStyles.popup)
            {
                fixedHeight = 0
            };
            if (OOEditorManager.OverrideLabelStyle != null)
                OOEditorManager.OverrideLabelStyle.ApplyToStyle(style);
            if (OOEditorManager.OverrideTextStyle != null)
                OOEditorManager.OverrideTextStyle.ApplyToStyle(style);

            // The line is four pixels larger than however large the style would end up being
            list.elementHeight = style.CalcHeight(new GUIContent(), 100) + 4;

            list.DoLayoutList();
        }
    }
}