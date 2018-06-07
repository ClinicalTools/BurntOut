using OOEditor;
using System;
using UnityEngine;

public class SceneNarrationEditor
{
    LabelField startNarrationLabel;
    TextField startNarrationField;
    LabelField endNarrationLabel;
    TextField endNarrationField;

    public SceneNarrationEditor(SceneNarrative sceneNarrative)
    {
        startNarrationLabel = new LabelField("Start Narration:");
        startNarrationLabel.Style.FontStyle = FontStyle.Bold;

        startNarrationField = new TextField(sceneNarrative.startNarration);
        startNarrationField.Changed += (object sender, EventArgs e) =>
        {
            var field = (TextField)sender;
            sceneNarrative.startNarration = field.Value;
        };

        endNarrationLabel = new LabelField("End Narration:");
        endNarrationLabel.Style.FontStyle = FontStyle.Bold;


        endNarrationField = new TextField(sceneNarrative.endNarration);
        endNarrationField.Changed += (object sender, EventArgs e) =>
        {
            var field = (TextField)sender;
            sceneNarrative.endNarration = field.Value;
        };
    }

    public void Draw()
    {
        startNarrationLabel.Draw();
        startNarrationField.Draw();



        endNarrationLabel.Draw();
        endNarrationField.Draw();
    }
}
