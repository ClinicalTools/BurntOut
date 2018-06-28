using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public static class SceneActors
    {
        private static void Init()
        {
            EditorApplication.hierarchyChanged += ResetActors;

            ResetActors();
        }

        private static void ResetActors()
        {
            var curActorObjects = Object.FindObjectsOfType<ActorObject>();
            // Use the previous array reference when possible
            if (actorObjects == null || actorObjects.Length != curActorObjects.Length)
                actorObjects = curActorObjects;
            else
                for (var i = 0; i < curActorObjects.Length; i++)
                    actorObjects[i] = curActorObjects[i];


            var validActors = new List<Actor>();
            foreach (var actorObject in actorObjects.Where(a => a.actor != null))
                validActors.Add(actorObject.actor);
            // Use previous reference when possible
            if (actors == null || actors.Length != validActors.Count)
                actors = validActors.ToArray();
            else
                for (var i = 0; i < validActors.Count; i++)
                    actors[i] = validActors[i];
            

            if (npcNames == null || npcNames.Length != actors.Length)
                npcNames = new string[actors.Length];
            if (actorNames == null || actorNames.Length != actors.Length + 1)
                actorNames = new string[actors.Length + 1];
            actorNames[0] = "Player";
            for (int i = 0; i < actors.Length; i++)
            {
                npcNames[i] = actors[i].name;
                actorNames[i + 1] = actors[i].name;
            }
        }

        private static ActorObject[] actorObjects;
        public static ActorObject[] ActorObjects
        {
            get
            {
                if (actorObjects == null)
                    Init();

                return actorObjects;
            }
        }


        private static Actor[] actors;
        public static Actor[] Actors
        {
            get
            {
                if (actors == null)
                    Init();

                return actors;
            }
        }

        public static int GetActorId(string actorName)
        {
            return Actors.FirstOrDefault(a => a.name == actorName)?.id ?? 0;
        }

        private static string[] npcNames;
        public static string[] NpcNames
        {
            get
            {
                if (npcNames == null)
                    Init();

                return npcNames;
            }
        }

        public static int GetNpcIndex(int actorId)
        {
            var index = -1;
            for (int i = 0; i < actors.Length; i++)
                if (actors[i].id == actorId)
                    index = i;

            return index;
        }

        private static string[] actorNames;
        public static string[] ActorNames
        {
            get
            {
                if (actorNames == null)
                    Init();

                return actorNames;
            }
        }

        public static int GetActorIndex(int actorId)
        {
            return GetNpcIndex(actorId) + 1;
        }
    }
}