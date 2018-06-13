using OOEditor;

public class ActorDrawer : GUIObjectDrawer<Actor>
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
