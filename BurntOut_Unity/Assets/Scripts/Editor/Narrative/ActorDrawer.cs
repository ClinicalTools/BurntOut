using OOEditor;

namespace Narrative.Inspector
{
    public class ActorDrawer : ClassDrawer<Actor>
    {
        TextField actorName;

        public ActorDrawer(Actor value)
        {
            Value = value;

            actorName = new TextField(value.name);
            actorName.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.name = e.Value;
            };
            actorName.Changed += OnChange;
        }

        public override void ResetValues()
        {
            actorName.Value = Value.name;
        }

        public override void Draw()
        {
            actorName.Draw();
        }
    }
}