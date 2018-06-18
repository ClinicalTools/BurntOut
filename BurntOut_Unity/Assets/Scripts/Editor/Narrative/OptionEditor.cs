﻿using OOEditor;
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


        public OptionEditor(Option value) : base(value)
        {
            Foldout = new Foldout(FoldoutName);
            Foldout.Style.FontColor = ResultColor;

            nameField = new TextField(Value.name, "Name:", "Name to be displayed in the editor");
            nameField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.name = e.Value;
                Foldout.Content.text = FoldoutName;
            };

            textField = new TextField(Value.text, "Text:", "Text to be displayed in game");
            textField.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.text = e.Value;
            };

            eventsFoldout = new Foldout("Events");
            eventsFoldout.Style.FontStyle = FontStyle.Bold;

            taskList = new ReorderableList<Task, TaskDrawer>(Value.Events);

            resultPopup = new EnumPopup(Value.result, "Result:");
            resultPopup.Changed += (object sender, ControlChangedArgs<Enum> e) =>
            {
                Value.result = (OptionResult)(e.Value);

                var color = ResultColor;
                Foldout.Style.FontColor = color;
                resultPopup.Style.FontColor = color;
            };

            feedbackLabel = new LabelField("Feedback:");
            feedback = new TextArea(Value.feedback);
            feedback.Changed += (object sender, ControlChangedArgs<string> e) =>
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

            feedbackLabel.Draw();
            feedback.Draw(Value.feedback);
        }
    }
}