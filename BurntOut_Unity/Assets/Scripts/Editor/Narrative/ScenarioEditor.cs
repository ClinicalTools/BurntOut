using OOEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of a scenario.
    /// </summary>
    public class ScenarioEditor : ClassDrawer<Scenario>
    {
        public static Scenario CurrentScenario { get; private set; }

        private readonly ReorderableList<Actor, ActorDrawer> actorsList;
        private readonly FoldoutList<Choice, ChoiceEditor> choiceList;

        private readonly TextField nameField;
        private readonly Foldout choicesFoldout;
        private readonly LabelField endNarrativeLabel;
        private readonly TextArea endNarrativeField;

        public ScenarioEditor(Scenario value) : base(value)
        {
            CurrentScenario = Value;

            nameField = new TextField(Value.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (sender, e) =>
            {
                Value.name = e.Value;
            };

            choicesFoldout = new Foldout("Choices");
            choicesFoldout.Style.FontStyle = FontStyle.Bold;
            choiceList = new FoldoutList<Choice, ChoiceEditor>(Value.Choices);

            endNarrativeLabel = new LabelField("End Narrative:");
            endNarrativeField = new TextArea(Value.endNarrative);
            endNarrativeField.Changed += (sender, e) =>
            {
                Value.endNarrative = e.Value;
            };
        }

        protected override void Display()
        {
            CurrentScenario = Value;

            nameField.Draw(Value.name);
            
            choicesFoldout.Draw();
            if (choicesFoldout.Value)
                using (Indent.Draw())
                    choiceList.Draw(Value.Choices);

            endNarrativeLabel.Draw();
            endNarrativeField.Draw(Value.endNarrative);
        }
    }
}