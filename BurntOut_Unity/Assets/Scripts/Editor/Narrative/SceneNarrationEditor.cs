using OOEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public class SceneNarrationEditor : ClassDrawer<SceneNarrative>
    {
        private readonly LabelField startNarrationLabel;
        private readonly TextArea startNarrationField;
        private readonly LabelField endNarrationLabel;
        private readonly TextArea endNarrationField;

        public SceneNarrationEditor(SceneNarrative value) : base(value)
        {
            startNarrationLabel = new LabelField("Start Narration:");
            startNarrationLabel.Style.FontStyle = FontStyle.Bold;

            startNarrationField = new TextArea(Value.startNarration);
            startNarrationField.Changed += (sender, e) =>
            {
                Value.startNarration = e.Value;
            };

            endNarrationLabel = new LabelField("End Narration:");
            endNarrationLabel.Style.FontStyle = FontStyle.Bold;


            endNarrationField = new TextArea(Value.endNarration);
            endNarrationField.Changed += (sender, e) =>
            {
                Value.endNarration = e.Value;
            };
        }

        protected override void Display()
        {
            startNarrationLabel.Draw();
            startNarrationField.Draw(Value.startNarration);

            BlankLine.Draw();

            endNarrationLabel.Draw();
            endNarrationField.Draw(Value.endNarration);
        }
    }
}