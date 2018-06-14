using OOEditor;

namespace Narrative.Inspector
{
    public class ActorDrawer : IGUIObjectDrawer<Actor>
    {
        TextField actorName;

        private Actor value;
        public Actor Value
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                actorName.Value = value.name;
            }
        }

        public ActorDrawer(Actor val)
        {
            value = val;

            actorName = new TextField(val.name);
            actorName.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.name = e.Value;
            };
        }

        public void Draw()
        {
            actorName.Draw();
        }
    }
}