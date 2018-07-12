using OOEditor;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public class ActorEventsEditor : FoldoutClassDrawer<ActorEvents>
    {
        public override Foldout Foldout { get; }

        // Event related fields
        private readonly LabelField eventsLabel;
        private readonly ReorderableList<Task, TaskEditor> eventsList;

        public ActorEventsEditor(ActorEvents value, int index) : base(value, index)
        {
            var actor = SceneActors.GetActor(Value.actorId);

            if (actor.icon != null)
                Foldout = new Foldout(actor.name, null, actor.icon.texture);
            else
                Foldout = new Foldout(actor.name, null);
            Foldout.Style.FontStyle = FontStyle.Bold;
            EditorApplication.hierarchyChanged += ResetFoldout;

            eventsLabel = new LabelField("Events:");
            eventsLabel.Style.FontStyle = FontStyle.Bold;
            eventsList = new ReorderableList<Task, TaskEditor>(Value.Events);
        }

        private void ResetFoldout()
        {
            var actor = SceneActors.GetActor(Value.actorId);
            Foldout.Content.text = actor.name;
            if (actor.icon != null)
                Foldout.Content.image = actor.icon.texture;
        }

        protected override void Display()
        {
            eventsLabel.Draw();
            eventsList.Draw();
        }
    }
}