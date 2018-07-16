using OOEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of an option for a given scenario.
    /// </summary>
    public class OptionEditor : FoldoutClassDrawer<Option>
    {
        protected string FoldoutName =>
            $"Option {Index + 1} - {Value.name}";
        public override Foldout Foldout { get; }

        protected Color? ResultColor
        {
            get
            {
                switch (Value.result)
                {
                    case OptionResult.CONTINUE:
                        return EditorColors.Green;
                    case OptionResult.TRY_AGAIN:
                        return EditorColors.Yellow;
                    case OptionResult.END:
                        return EditorColors.Red;
                    default:
                        return null;
                }
            }
        }
        protected Color HealthColor
        {
            get
            {
                if (Value.healthChange > 10)
                    return EditorColors.Green;
                else if (Value.healthChange > 0)
                    return EditorColors.YellowGreen;
                else if (Value.healthChange >= -10)
                    return EditorColors.Yellow;
                else
                    return EditorColors.Red;
            }
        }

        private Foldout eventsFoldout;
        private ReorderableList<Task, TaskEditor> taskList;

        private readonly TextField nameField;
        private readonly TextField textField;

        private readonly EnumPopup resultPopup;
        private readonly TextField healthChangeField;

        private readonly LabelField feedbackLabel;
        private readonly TextArea feedback;


        public OptionEditor(Option value, int index) : base(value, index)
        {
            Foldout = new Foldout(FoldoutName);
            Foldout.Style.FontColor = HealthColor;

            IndexChanged += (sender, e) =>
            {
                Foldout.Content.text = FoldoutName;
            };

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

            taskList = new ReorderableList<Task, TaskEditor>(Value.Events);

            resultPopup = new EnumPopup(Value.result, "Result:");
            resultPopup.Style.FontColor = ResultColor;
            resultPopup.Changed += (sender, e) =>
            {
                Value.result = (OptionResult)(e.Value);

                var color = ResultColor;
                Foldout.Style.FontColor = color;
                resultPopup.Style.FontColor = color;
            };

            healthChangeField = new TextField(Value.HealthChangeStr, "Health Change:");
            healthChangeField.Style.FontColor = HealthColor;
            healthChangeField.Changed += (sender, e) =>
            {
                Value.HealthChangeStr = e.Value;
                Foldout.Style.FontColor = HealthColor;
                healthChangeField.Style.FontColor = HealthColor;
            };

            feedback = new TextArea(Value.feedback);
            feedback.Changed += (sender, e) =>
            {
                Value.feedback = e.Value;
            };
            feedbackLabel = new LabelField(feedback, "Feedback:");
        }

        protected override void Display()
        {
            nameField.Draw(Value.name);
            textField.Draw(Value.text);

            eventsFoldout.Draw();
            if (eventsFoldout.Value)
                using (Indent.Draw())
                    taskList.Draw(Value.Events);

            //resultPopup.Draw(Value.result);
            healthChangeField.Draw(Value.HealthChangeStr);

            feedbackLabel.Draw();
            feedback.Draw(Value.feedback);
        }
    }
}