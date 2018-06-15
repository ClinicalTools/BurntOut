using System.Collections.Generic;

namespace OOEditor
{
    public class FoldoutList<T, TDrawer> : GUIList<T, TDrawer>
        where TDrawer : FoldoutClassDrawer<T>
    {
        public FoldoutList(List<T> value) : base(value) { }
        
        protected override void Display()
        {
            for (var i = 0; i < Value.Count; i++)
            {
                using (Horizontal.Draw())
                {
                    Drawers[i].DrawFoldout();

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

                if (i < Drawers.Count && Drawers[i].Expanded)
                    using (Indent.Draw())
                    using (GUIContainer.Draw())
                        Drawers[i].Draw();
            }

            AddButton.Draw();
        }
    }
}