using System;
using UnityEngine;

namespace Narrative
{
    [Serializable]
    public class Actor
    {
        // Number to represent this actor.
        [SerializeField]
        //[HideInInspector]
        public int id;

        [SerializeField]
        public string name;

        public Texture2D icon;
        public Sprite normal;

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

                if (hash == 0)
                {
                    hash++;
                    flag = false;
                }

                foreach (var actor in actors)
                    if (actor.id == hash)
                    {
                        hash++;
                        flag = false;
                        break;
                    }
            }

            id = hash;
        }
    }
}