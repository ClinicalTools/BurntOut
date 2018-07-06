using OOEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of a choice for a given scenario.
    /// </summary>
    public class ChoiceEditor : FoldoutClassDrawer<Choice>
    {
        protected string FoldoutName => $"{(Value.isChoice ? "Choice" : "Events")} - {Value.name}";
        public override Foldout Foldout { get; }

        // Basic info fields
        private readonly TextField nameField;
        private readonly Toggle continueLast;
        private readonly Foldout triggersFoldout;
        private readonly ReorderableList<Trigger, TriggerDrawer> triggerList;

        // Actual event related fields
        private readonly Foldout eventsFoldout;
        private readonly ReorderableList<Task, TaskDrawer> taskList;

        // Choice related fields
        private readonly Toggle choiceToggle;
        private readonly TextField textField;
        private readonly Foldout optionsFoldout;
        private readonly FoldoutList<Option, OptionEditor> optionList;

        public ChoiceEditor(Choice value, int index) : base(value, index)
        {
            Foldout = new Foldout(FoldoutName);
            ResetFoldout();

            nameField = new TextField(Value.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (sender, e) =>
            {
                Value.name = e.Value;
                Foldout.Content.text = FoldoutName;
            };

            continueLast = new Toggle(Value.continueLast, "Continue Last Event:",
                "Immediately starts the events after the previous events");
            continueLast.Changed += (sender, e) =>
            {
                Value.continueLast = e.Value;
            };
            triggersFoldout = new Foldout("Triggers");
            triggersFoldout.Style.FontStyle = FontStyle.Bold;
            triggerList = new ReorderableList<Trigger, TriggerDrawer>(Value.Triggers);

            eventsFoldout = new Foldout(false, "Events");
            eventsFoldout.Style.FontStyle = FontStyle.Bold;
            taskList = new ReorderableList<Task, TaskDrawer>(Value.Events);

            choiceToggle = new Toggle(Value.isChoice, "Choice:");
            choiceToggle.Changed += (sender, e) =>
            {
                Value.isChoice = e.Value;
                Foldout.Content.text = FoldoutName;
                ResetFoldout();
            };

            textField = new TextField(Value.text, "Prompt:", "Choice prompt displayed in game");
            textField.Changed += (sender, e) =>
            {
                Value.text = e.Value;
            };

            optionsFoldout = new Foldout(false, "Options");
            optionsFoldout.Style.FontStyle = FontStyle.Bold;
            optionList = new FoldoutList<Option, OptionEditor>(Value.Options);
            optionList.Changed += (sender, e) =>
            {
                ResetFoldout();
            };
        }

        private void ResetFoldout()
        {
            if (!Value.isChoice || Value.Options.Exists(option => option.result == OptionResult.CONTINUE))
            {
                Foldout.Style.FontColor = null;
                Foldout.Content.tooltip = "";
            }
            else
            {
                Foldout.Style.FontColor = EditorColors.Error;
                Foldout.Content.tooltip = "No continue option";
            }
        }

        protected override void Display()
        {
            nameField.Draw(Value.name);

            // Trigger drawing
            triggersFoldout.Draw();
            if (triggersFoldout.Value)
                using (Indent.Draw())
                {
                    continueLast.Draw(Value.continueLast);
                    if (!Value.continueLast)
                        triggerList.Draw(Value.Triggers);
                }

            // Event drawing
            eventsFoldout.Draw();
            if (eventsFoldout.Value)
                using (Indent.Draw())
                    taskList.Draw(Value.Events);

            // Choice drawing
            choiceToggle.Draw();
            if (Value.isChoice)
                using (Indent.Draw())
                {
                    textField.Draw(Value.text);

                    optionsFoldout.Draw();
                    if (optionsFoldout.Value)
                        using (Indent.Draw())
                            optionList.Draw(Value.Options);
                }
        }
    }
}