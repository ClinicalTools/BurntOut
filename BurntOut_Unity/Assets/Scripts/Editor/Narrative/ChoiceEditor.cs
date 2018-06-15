using OOEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of a choice for a given scenario.
    /// </summary>
    public class ChoiceEditor : FoldoutClassDrawer<Choice>
    {
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


        public static Choice CurrentChoice { get; private set; }

        private readonly TextField nameField;
        private readonly Foldout eventsFoldout;
        private readonly ReorderableList<Task, TaskDrawer> taskList;
        private readonly TextField textField;
        private readonly Foldout optionsFoldout;
        private readonly FoldoutList<Option, OptionEditor> optionList;

        public ChoiceEditor(Choice choice)
        {
            CurrentChoice = choice;
            Value = choice;

            Foldout = new Foldout(FoldoutName);
            Foldout.Style.FontColor = FoldoutColor;

            nameField = new TextField(choice.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                choice.name = e.Value;
                Foldout.Content.text = FoldoutName;
            };

            textField = new TextField(choice.text, "Text:", "Text to be displayed in game");
            textField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                choice.text = e.Value;
            };

            eventsFoldout = new Foldout(false, "Events");
            eventsFoldout.Style.FontStyle = FontStyle.Bold;
            taskList = new ReorderableList<Task, TaskDrawer>(Value.Events);

            optionsFoldout = new Foldout(false, "Options");
            optionsFoldout.Style.FontStyle = FontStyle.Bold;
            optionList = new FoldoutList<Option, OptionEditor>(Value.Options);
            optionList.Changed += (object sender, ListChangedArgs<Option> e) =>
            {
                Foldout.Style.FontColor = FoldoutColor;
            };
        }

        public override void ResetValues()
        {
            nameField.Value = Value.name;
            textField.Value = Value.text;
            taskList.Value = Value.Events;
            optionList.Value = Value.Options;
        }

        public override void Draw()
        {
            CurrentChoice = Value;

            nameField.Draw();

            eventsFoldout.Draw();
            if (eventsFoldout.Value)
                using (Indent.Draw())
                    taskList.Draw();

            textField.Draw();

            optionsFoldout.Draw();
            if (optionsFoldout.Value)
                using (Indent.Draw())
                    optionList.Draw();
        }
    }
}