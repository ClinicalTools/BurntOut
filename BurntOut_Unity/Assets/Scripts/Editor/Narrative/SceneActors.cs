using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace Narrative.Inspector
{
    public static class SceneActors
    {
        private const string NARRATOR_NAME = "NARRATOR";
        private const string PLAYER_NAME = "PLAYER";

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
            if (actorNames == null || actorNames.Length != actors.Length + 2)
                actorNames = new string[actors.Length + 2];
            actorNames[0] = NARRATOR_NAME;
            actorNames[1] = PLAYER_NAME;
            for (int i = 0; i < actors.Length; i++)
            {
                npcNames[i] = actors[i].name;
                actorNames[i + 2] = actors[i].name;
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
            if (actorName == NARRATOR_NAME)
                return Actor.NARRATOR_ID;
            else if (actorName == PLAYER_NAME)
                return Actor.PLAYER_ID;
            else
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
            for (int i = 0; i < Actors.Length; i++)
                if (Actors[i].id == actorId)
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
            if (actorId == Actor.NARRATOR_ID)
                return 0;
            else if (actorId == Actor.PLAYER_ID)
                return 1;
            
            var npcIndex = GetNpcIndex(actorId);

            if (npcIndex >= 0)
                return npcIndex + 2;
            else
                return -1;
        }
    }
}