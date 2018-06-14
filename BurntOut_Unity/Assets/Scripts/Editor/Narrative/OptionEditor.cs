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
        protected override string FoldoutName => $"Option {(ChoiceEditor.CurrentChoice.Options.IndexOf(Value) + 1)} - {Value.name}";
        protected override Color? FoldoutColor
        {
            get
            {
                switch (Value.result)
                {
                    case OptionResults.CONTINUE:
                        return EditorColors.LightGreen;
                    case OptionResults.TRY_AGAIN:
                        return EditorColors.LightYellow;
                    case OptionResults.END:
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

            nameField = new TextField(option.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.name = e.Value;
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
                Value.result = (OptionResults)(e.Value);
                SetResultPopupColor();
            };
            SetResultPopupColor();

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

        private void SetResultPopupColor()
        {
            switch (Value.result)
            {
                case OptionResults.CONTINUE:
                    resultPopup.Style.FontColor = EditorColors.LightGreen;
                    break;
                case OptionResults.TRY_AGAIN:
                    resultPopup.Style.FontColor = EditorColors.LightYellow;
                    break;
                case OptionResults.END:
                    resultPopup.Style.FontColor = EditorColors.LightRed;
                    break;
            }
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