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

        private readonly FoldoutList<Choice, ChoiceEditor> choiceList;

        private TextField nameField;
        private Foldout actorsFoldout, choicesFoldout;
        private LabelField endNarrativeLabel;
        private TextArea endNarrativeField;

        private GUIList<Actor, ActorDrawer> actorsList;

        public ScenarioEditor(Scenario value) : base(value)
        {
            CurrentScenario = Value;

            nameField = new TextField(Value.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                base.Value.name = e.Value;
            };

            actorsFoldout = new Foldout("Actors");
            actorsFoldout.Style.FontStyle = FontStyle.Bold;
            actorsList = new GUIList<Actor, ActorDrawer>(base.Value.Actors)
            {
                DefaultElement = () => { return new Actor(base.Value.Actors.ToArray()); }
            };

            choicesFoldout = new Foldout("Choices");
            choicesFoldout.Style.FontStyle = FontStyle.Bold;
            choiceList = new FoldoutList<Choice, ChoiceEditor>(base.Value.Choices);

            endNarrativeLabel = new LabelField("End Narrative:");
            endNarrativeField = new TextArea(base.Value.endNarrative);
            endNarrativeField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                base.Value.endNarrative = e.Value;
            };
        }

        protected override void Display()
        {
            CurrentScenario = Value;

            nameField.Draw(Value.name);

            actorsFoldout.Draw();
            if (actorsFoldout.Value)
                using (Indent.Draw())
                    actorsList.Draw(Value.Actors);

            choicesFoldout.Draw();
            if (choicesFoldout.Value)
                using (Indent.Draw())
                    choiceList.Draw(Value.Choices);

            endNarrativeLabel.Draw();
            endNarrativeField.Draw(Value.endNarrative);
        }
    }
}