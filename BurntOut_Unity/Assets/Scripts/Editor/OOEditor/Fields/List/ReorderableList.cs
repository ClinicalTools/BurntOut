using OOEditor.Internal;
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

            list = new UnityEditorInternal.ReorderableList(Value, typeof(T))
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
                    
                    //Value[index] = drawers[index].Value;
                }
            };
        }
        
        public void Draw()
        {
            list.elementHeight = EditorStyles.popup.CalcHeight(new GUIContent(" "), 100) + 4;

            using (new Vertical())
                list.DoLayoutList();
                
            //for (int i = 0; i < Value.Count; i++)
                //Value[i] = drawers[i].Value;
        }
    }
}