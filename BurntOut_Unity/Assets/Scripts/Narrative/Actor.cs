using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Actor
{
    [SerializeField]
    public string name;

    // Number to represent this actor.
    [SerializeField]
    public int id;

    public Actor(Actor[] actors)
    {
        Debug.Log(DateTime.Now.GetHashCode());
        // Using hash of current time to get a number that will hopefully be unique
        var hash = DateTime.Now.GetHashCode();

        // Ensure no other player in the passed list has the same id
        // a true flag means that the code hasn't hit another actor with the same id
        bool flag = false;
        while (!flag)
        {
            flag = true;
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