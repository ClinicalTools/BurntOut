using OOEditor;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class ActorPopupWindow
{
    public List<int> ActorIds { get; set; }

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


    private readonly List<ToggleLeft> actorToggles = new List<ToggleLeft>();
    private List<Actor> allActors = new List<Actor>();

    public ActorPopupWindow(List<int> actorIds)
    {
        ActorIds = actorIds;

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
            {
                if (allActors[i] != actorList[i] || 
                    actorToggles[i].Content.text.Trim() != actorList[i].name.Trim() ||
                    actorToggles[i].Content.image != actorList[i].icon)
                {
                    actorsChanged = true;
                    break;
                }
            }

        if (!actorsChanged)
            return;

        allActors = actorList;
        ResetToggles();
    }

    private void ResetToggles()
    {
        actorToggles.Clear();

        foreach (var actor in allActors)
        {
            var toggle = new ToggleLeft(
                ActorIds.Contains(actor.id), " " + actor.name, null, actor.icon);
            var id = actor.id;
            toggle.Pressed += (o, sender) =>
            {
                if (!sender.Value)
                    ActorIds.Remove(id);
                else if (!ActorIds.Contains(id))
                    ActorIds.Add(id);
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
            selected = EditorGUILayout.ToggleLeft(new GUIContent(" " + actor.name, actor.icon), selected);

    }

    public void Draw(List<int> actors)
    {
        if (actors != ActorIds)
        {
            ActorIds = actors;
            ResetToggles();
        }
        UpdateAllActors();

        using (GUIContainer.Draw())
            foreach (var toggle in actorToggles)
                toggle.Draw();
    }
}
