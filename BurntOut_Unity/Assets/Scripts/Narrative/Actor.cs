using System;
using UnityEngine;

namespace Narrative
{
    [Serializable]
    public class Actor
    {
        /// <summary>
        /// ID values representing entities taking similar roles to actors without actually using an Actor script.
        /// </summary>
        /// <remarks>Changing these values will break all references to them in all scenarios.</remarks>
        public const int NARRATOR_ID = -1;
        public const int PLAYER_ID = 1;


        // Number to represent this actor.
        [SerializeField]
        //[HideInInspector]
        public int id;

        [SerializeField]
        public string name;

        public Sprite icon;
        public Sprite neutral, happy, sad, angry, scared;

        public Actor(Actor[] actors)
        {
            // Using hash of current time to get a number that will hopefully be unique
            var hash = DateTime.Now.GetHashCode();

            // Ensure no other player in the passed list has the same id
            // a true flag means that the code hasn't hit another actor with the same id
            bool flag = false;
            while (!flag)
            {
                flag = true;

                if (hash == NARRATOR_ID || hash == PLAYER_ID || hash == 0)
                {
                    hash++;
                    flag = false;
                    continue;
                }

                foreach (var actor in actors)
                {
                    if (actor.id == hash)
                    {
                        hash++;
                        flag = false;
                        break;
                    }
                }
            }

            id = hash;
        }
    }
}