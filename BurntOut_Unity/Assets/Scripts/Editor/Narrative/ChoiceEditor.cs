using OOEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of a choice for a given scenario.
    /// </summary>
    public class ChoiceEditor : FoldoutClassDrawer<Choice>
    {
        public static Choice CurrentChoice { get; private set; }
        
        protected string FoldoutName =>
            $"Choice {ScenarioEditor.CurrentScenario.Choices.IndexOf(Value) + 1} - {Value.name}";
        protected override Foldout Foldout { get; }

        protected Color? FoldoutColor
        {
            get
            {
                if (Value.Options.Exists(option => option.result == OptionResult.CONTINUE))
                    return null;
                else
                    return EditorColors.Red;
            }
        }

        private readonly TextField nameField;
        private readonly Foldout eventsFoldout;
        private readonly ReorderableList<Task, TaskDrawer> taskList;
        private readonly TextField textField;
        private readonly Foldout optionsFoldout;
        private readonly FoldoutList<Option, OptionEditor> optionList;

        public ChoiceEditor(Choice value) : base(value)
        {
            CurrentChoice = Value;

            Foldout = new Foldout(FoldoutName);
            Foldout.Style.FontColor = FoldoutColor;

            nameField = new TextField(Value.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (sender, e) =>
            {
                Value.name = e.Value;
                Foldout.Content.text = FoldoutName;
            };

            textField = new TextField(Value.text, "Text:", "Text to be displayed in game");
            textField.Changed += (sender, e) =>
            {
                Value.text = e.Value;
            };

            eventsFoldout = new Foldout(false, "Events");
            eventsFoldout.Style.FontStyle = FontStyle.Bold;
            taskList = new ReorderableList<Task, TaskDrawer>(Value.Events);

            optionsFoldout = new Foldout(false, "Options");
            optionsFoldout.Style.FontStyle = FontStyle.Bold;
            optionList = new FoldoutList<Option, OptionEditor>(Value.Options);
            optionList.Changed += (sender, e) =>
            {
                Foldout.Style.FontColor = FoldoutColor;
            };
        }

        protected override void Display()
        {
            CurrentChoice = Value;

            nameField.Draw(Value.name);

            eventsFoldout.Draw();
            if (eventsFoldout.Value)
                using (Indent.Draw())
                    taskList.Draw(Value.Events);

            textField.Draw(Value.text);

            optionsFoldout.Draw();
            if (optionsFoldout.Value)
                using (Indent.Draw())
                    optionList.Draw(Value.Options);
        }
    }
}