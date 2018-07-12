using OOEditor;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    /// <summary>
    /// Manages the editing of an interactable object in a scene.
    /// </summary>
    public class InteractableEditor : FoldoutClassDrawer<Interactable>
    {
        public override Foldout Foldout { get; }

        // Event related fields
        private readonly LabelField eventsLabel;
        private readonly ReorderableList<Task, TaskDrawer> eventsList;

        public InteractableEditor(Interactable value, int index) : base(value, index)
        {
            Foldout = new Foldout(Value.name);
            EditorApplication.hierarchyChanged += () => 
            {
                if (Value != null)
                    Foldout.Content.text = Value?.name;
            };

            eventsLabel = new LabelField("Events:");
            eventsLabel.Style.FontStyle = FontStyle.Bold;
            eventsList = new ReorderableList<Task, TaskDrawer>(Value.Events);
        }

        protected override void Display()
        {
            // Allows the scene to save changes and 'undo' to be possible
            Undo.RecordObject(Value, "Interactable change");

            eventsLabel.Draw();
            eventsList.Draw();
        }
    }
}