﻿using OOEditor;

namespace Narrative.Inspector
{
    public class ActorDrawer : ClassDrawer<Actor>
    {
        TextField actorName;

        public ActorDrawer(Actor value) : base(value)
        {
            actorName = new TextField(Value.name);
            actorName.Changed += (object sender, ControlChangedArgs<string> e) =>
            {
                Value.name = e.Value;
            };
        }

        protected override void Display()
        {
            actorName.Draw(Value.name);
        }
    }
}