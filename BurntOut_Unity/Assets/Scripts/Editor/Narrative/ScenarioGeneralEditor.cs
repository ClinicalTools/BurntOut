using OOEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public class ScenarioGeneralEditor : ClassDrawer<Scenario>
    {
        public static Scenario CurrentScenario { get; private set; }

        private readonly TextField nameField;
        private readonly Toggle startNarrativeToggle, endNarrativeToggle;
        private readonly TextArea startNarrativeField, endNarrativeField;

        public ScenarioGeneralEditor(Scenario value) : base(value)
        {
            CurrentScenario = Value;

            nameField = new TextField(Value.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (sender, e) =>
            {
                Value.name = e.Value;
            };

            startNarrativeToggle = new Toggle(Value.hasStartNarrative, "Start Narrative:");
            startNarrativeToggle.Changed += (sstarter, e) =>
            {
                Value.hasStartNarrative = e.Value;
            };
            startNarrativeField = new TextArea(Value.startNarrative);
            startNarrativeField.Changed += (sstarter, e) =>
            {
                Value.startNarrative = e.Value;
            };

            endNarrativeToggle = new Toggle(Value.hasEndNarrative, "End Narrative:");
            endNarrativeToggle.Changed += (sender, e) =>
            {
                Value.hasEndNarrative = e.Value;
            };
            endNarrativeField = new TextArea(Value.endNarrative);
            endNarrativeField.Changed += (sender, e) =>
            {
                Value.endNarrative = e.Value;
            };
        }

        protected override void Display()
        {
            CurrentScenario = Value;

            //nameField.Draw(Value.name);

            startNarrativeToggle.Draw();
            if (Value.hasStartNarrative)
                using (Indent.Draw())
                    startNarrativeField.Draw(Value.startNarrative);

            endNarrativeToggle.Draw();
            if (Value.hasEndNarrative)
                using (Indent.Draw())
                    endNarrativeField.Draw(Value.endNarrative);
        }
    }
}