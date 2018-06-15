using OOEditor;
using System;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of an option for a given scenario.
    /// </summary>
    public class OptionEditor : FoldoutClassDrawer<Option>
    {
        protected string FoldoutName =>
            $"Option {(ChoiceEditor.CurrentChoice.Options.IndexOf(Value) + 1)} - {Value.name}";
        protected override Foldout Foldout { get; }

        protected Color? ResultColor
        {
            get
            {
                switch (Value.result)
                {
                    case OptionResult.CONTINUE:
                        return EditorColors.LightGreen;
                    case OptionResult.TRY_AGAIN:
                        return EditorColors.LightYellow;
                    case OptionResult.END:
                        return EditorColors.LightRed;
                    default:
                        return null;
                }
            }
        }

        private Foldout eventsFoldout;
        private ReorderableList<Task, TaskDrawer> taskList;

        private readonly TextField nameField;
        private readonly TextField textField;

        private readonly EnumPopup resultPopup;

        private readonly LabelField feedbackLabel;
        private readonly TextArea feedback;


        public OptionEditor(Option option)
        {
            Value = option;

            Foldout = new Foldout(FoldoutName);
            Foldout.Style.FontColor = ResultColor;

            nameField = new TextField(option.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.name = e.Value;
                Foldout.Content.text = FoldoutName;
            };

            textField = new TextField(option.text, "Text:", "Text to be displayed in game");
            textField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.text = e.Value;
            };

            eventsFoldout = new Foldout("Events");
            eventsFoldout.Style.FontStyle = FontStyle.Bold;

            taskList = new ReorderableList<Task, TaskDrawer>(option.Events);

            resultPopup = new EnumPopup(Value.result, "Result:");
            resultPopup.Changed += (object sender, ControlChangedArgs<Enum> e) =>
            {
                Value.result = (OptionResult)(e.Value);

                var color = ResultColor;
                Foldout.Style.FontColor = color;
                resultPopup.Style.FontColor = color;
            };

            feedbackLabel = new LabelField("Feedback:");
            feedback = new TextArea(option.feedback);
            feedback.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.feedback = e.Value;
            };
        }

        public override void ResetValues()
        {
            nameField.Value = Value.name;
            textField.Value = Value.text;
            taskList.Value = Value.Events;
            resultPopup.Value = Value.result;
            feedback.Value = Value.feedback;
        }

        public override void Draw()
        {
            nameField.Draw();
            textField.Draw();

            eventsFoldout.Draw();
            if (eventsFoldout.Value)
                using (Indent.Draw())
                    taskList.Draw();

            resultPopup.Draw();

            feedbackLabel.Draw();
            feedback.Draw();
        }
    }
}