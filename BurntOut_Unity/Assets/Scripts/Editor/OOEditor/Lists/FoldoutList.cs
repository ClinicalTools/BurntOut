using System.Collections.Generic;

namespace OOEditor
{
    public class FoldoutList<T, TDrawer> : GUIList<T, TDrawer>
        where TDrawer : FoldoutClassDrawer<T>
    {
        public FoldoutList(List<T> value) : base(value) { }
        
        protected override void Display()
        {
            for (var i = 0; i < List.Count; i++)
            {
                using (Horizontal.Draw())
                {
                    Drawers[i].DrawFoldout();

                    if (i > 0)
                        UpButtons[i - 1].Draw();
                    else
                        ButtonSpace.Draw();

                    if (i < List.Count - 1)
                        DownButtons[i].Draw();
                    else
                        ButtonSpace.Draw();

                    DelButtons[i].Draw();
                }

                if (i < Drawers.Count && Drawers[i].Expanded)
                    using (Indent.Draw())
                    using (GUIContainer.Draw())
                        Drawers[i].Draw(List[i]);
            }

            AddButton.Draw();
        }
    }
}