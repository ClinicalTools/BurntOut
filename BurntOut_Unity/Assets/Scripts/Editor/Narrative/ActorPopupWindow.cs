using OOEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ActorPopupWindow
{
    public List<Actor> Actors { get; set; }

    public static GenericMenu ActorMenu(IEnumerable<Actor> actors)
    {
        var gameActors = Resources.FindObjectsOfTypeAll<ActorObject>();

        var actorList = new List<Actor>();
        foreach (var actorObj in gameActors.Where(a => a.actor != null))
            actorList.Add(actorObj.actor);

        var menu = new GenericMenu();

        foreach (var actor in actorList)
            menu.AddItem(new GUIContent(actor.name, actor.icon), actors.Contains(actor), x);

        return menu;
    }

    private static void x()
    {
        Debug.Log("cool");
    }


    private readonly List<Toggle> actorToggles = new List<Toggle>();
    private List<Actor> allActors = new List<Actor>();

    public ActorPopupWindow(List<Actor> actors)
    {
        Actors = actors;
        var actorObjs = Resources.FindObjectsOfTypeAll<ActorObject>();

        foreach (var actorObj in actorObjs.Where(a => a.actor != null))
            allActors.Add(actorObj.actor);

        ResetToggles();
    }

    private void UpdateAllActors()
    {
        var actorObjs = Resources.FindObjectsOfTypeAll<ActorObject>();

        var actorList = new List<Actor>();
        foreach (var actorObj in actorObjs.Where(a => a.actor != null))
            actorList.Add(actorObj.actor);

        bool actorsChanged = actorList.Count != allActors.Count;
        if (!actorsChanged)
            for (var i = 0; i < actorList.Count; i++)
                if (actorList[i] != allActors[i])
                {
                    actorsChanged = true;
                    break;
                }

        if (!actorsChanged)
            return;

        allActors = actorList;
    }

    private void ResetToggles()
    {
        Debug.Log("resetting toggles");

        actorToggles.Clear();

        foreach (var actor in allActors)
        {
            var toggle = new Toggle(Actors.Contains(actor), actor.name);
            var thisActor = actor;
            toggle.Changed += (o, sender) =>
            {
                if (sender.Value)
                    Actors.Remove(thisActor);
                else
                    Actors.Add(thisActor);
            };
            actorToggles.Add(toggle);
        }
    }

    static bool selected;
    private void Test()
    {
        var gameActors = Resources.FindObjectsOfTypeAll<ActorObject>();

        var actorList = new List<Actor>();
        foreach (var actorObj in gameActors.Where(a => a.actor != null))
            actorList.Add(actorObj.actor);

        foreach (var actor in actorList)
            selected = EditorGUILayout.ToggleLeft(new GUIContent(actor.name, actor.icon), selected);

    }

    public void Draw(List<Actor> actors)
    {
        if (actors != Actors)
        {
            Actors = actors;
            ResetToggles();
        }
        UpdateAllActors();

        foreach (var toggle in actorToggles)
            toggle.Draw();
    }
}
