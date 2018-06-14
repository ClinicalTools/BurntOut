using OOEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of a scenario.
    /// </summary>
    public class ScenarioEditor
    {
        public static Scenario CurrentScenario { get; private set; }

        public readonly Scenario scenario;

        private readonly FoldoutList<Choice, ChoiceEditor> choiceList;

        private TextField nameField;
        private Foldout actorsFoldout, choicesFoldout;
        private LabelField endNarrativeLabel;
        private TextArea endNarrativeField;

        private GUIList<Actor, ActorDrawer> actorsList;

        public ScenarioEditor(Scenario scenario)
        {
            CurrentScenario = scenario;
            this.scenario = scenario;

            nameField = new TextField(scenario.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                scenario.name = e.Value;
            };

            actorsFoldout = new Foldout("Actors");
            actorsFoldout.Style.FontStyle = FontStyle.Bold;
            actorsList = new GUIList<Actor, ActorDrawer>(scenario.Actors)
            {
                DefaultElement = () => { return new Actor(scenario.Actors.ToArray()); }
            };


            choicesFoldout = new Foldout("Choices");
            choicesFoldout.Style.FontStyle = FontStyle.Bold;
            choiceList = new FoldoutList<Choice, ChoiceEditor>(scenario.Choices);

            endNarrativeLabel = new LabelField("End Narrative:");
            endNarrativeField = new TextArea(scenario.endNarrative);
            endNarrativeField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                scenario.endNarrative = e.Value;
            };
        }

        public void Draw()
        {
            CurrentScenario = scenario;

            nameField.Draw();

            actorsFoldout.Draw();
            if (actorsFoldout.Value)
                using (Indent.Draw())
                    actorsList.Draw();

            choicesFoldout.Draw();
            if (choicesFoldout.Value)
                using (Indent.Draw())
                    choiceList.Draw();

            endNarrativeLabel.Draw();
            endNarrativeField.Draw();
        }
    }
}