using OOEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public class SceneNarrationEditor
    {
        LabelField startNarrationLabel;
        TextArea startNarrationField;
        LabelField endNarrationLabel;
        TextArea endNarrationField;

        public SceneNarrationEditor(SceneNarrative sceneNarrative)
        {
            startNarrationLabel = new LabelField("Start Narration:");
            startNarrationLabel.Style.FontStyle = FontStyle.Bold;

            startNarrationField = new TextArea(sceneNarrative.startNarration);
            startNarrationField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                sceneNarrative.startNarration = e.Value;
            };

            endNarrationLabel = new LabelField("End Narration:");
            endNarrationLabel.Style.FontStyle = FontStyle.Bold;


            endNarrationField = new TextArea(sceneNarrative.endNarration);
            endNarrationField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                sceneNarrative.endNarration = e.Value;
            };
        }

        public void Draw()
        {
            startNarrationLabel.Draw();
            startNarrationField.Draw();

            BlankLine.Draw();

            endNarrationLabel.Draw();
            endNarrationField.Draw();
        }
    }
}