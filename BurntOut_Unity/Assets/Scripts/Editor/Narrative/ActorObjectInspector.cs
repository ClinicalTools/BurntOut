using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ActorObject))]
public class ActorObjectInspector : Editor
{
    ActorObject obj;
    public void OnEnable()
    {
        obj = (ActorObject)target;
        if (obj.actor == null)
        {
            var actors = Resources.FindObjectsOfTypeAll<ActorObject>();

            var actorList = new List<Actor>();
            foreach (var actorObj in actors.Where(a => a.actor != null))
                actorList.Add(actorObj.actor);

            obj.actor = new Actor(actorList.ToArray());
        }

    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
    }
}