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
        private readonly TextField healthChangeField;

        private readonly LabelField feedbackLabel;
        private readonly TextArea feedback;


        public OptionEditor(Option value) : base(value)
        {
            Foldout = new Foldout(FoldoutName);
            Foldout.Style.FontColor = ResultColor;

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

            eventsFoldout = new Foldout("Events");
            eventsFoldout.Style.FontStyle = FontStyle.Bold;

            taskList = new ReorderableList<Task, TaskDrawer>(Value.Events);

            resultPopup = new EnumPopup(Value.result, "Result:");
            resultPopup.Changed += (sender, e) =>
            {
                Value.result = (OptionResult)(e.Value);

                var color = ResultColor;
                Foldout.Style.FontColor = color;
                resultPopup.Style.FontColor = color;
            };

            healthChangeField = new TextField(Value.HealthChangeStr, "Health Change:");
            healthChangeField.Changed += (sender, e) =>
            {
                Value.HealthChangeStr = e.Value;
                if (Value.healthChange > 0)
                    healthChangeField.Style.FontColor = EditorColors.LightGreen;
                else if (Value.healthChange < 0)
                    healthChangeField.Style.FontColor = EditorColors.LightRed;
                else 
                    healthChangeField.Style.FontColor = EditorColors.LightYellow;
            };

            feedbackLabel = new LabelField("Feedback:");
            feedback = new TextArea(Value.feedback);
            feedback.Changed += (sender, e) =>
            {
                Value.feedback = e.Value;
            };
        }

        protected override void Display()
        {
            nameField.Draw(Value.name);
            textField.Draw(Value.text);

            eventsFoldout.Draw();
            if (eventsFoldout.Value)
                using (Indent.Draw())
                    taskList.Draw(Value.Events);

            resultPopup.Draw(Value.result);
            healthChangeField.Draw(Value.HealthChangeStr);

            feedbackLabel.Draw();
            feedback.Draw(Value.feedback);
        }
    }
}