using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
[XmlRoot("Scenario")]
public class Scenario
{
    // Number to represent this actor.
    [SerializeField]
    public int id;

    [XmlAttribute("name")]
    public string name;

    [SerializeField]
    [XmlArray("Actors"), XmlArrayItem("Actor")]
    private List<Actor> actors;
    public List<Actor> Actors
    {
        get
        {
            if (actors == null)
                actors = new List<Actor>();

            return actors;
        }
    }
    public int ActorIndex(int id)
    {
        for (int i = 0; i < actors.Count; i++)
            if (actors[i].id == id)
                return i;

        return -1;
    }
    public Actor GetActor(int id)
    {
        foreach (Actor actor in actors)
            if (actor.id == id)
                return actor;

        return null;
    }
    public string[] ActorNames()
    {
        var arr = new string[actors.Count];
        for (int i = 0; i < arr.Length; i++)
            arr[i] = actors[i].name;

        return arr;
    }

    [SerializeField]
    [XmlArray("Choices"), XmlArrayItem("Choice")]
    private List<Choice> choices;
    public List<Choice> Choices
    {
        get
        {
            if (choices == null)
                choices = new List<Choice>();

            return choices;
        }
    }

    [XmlAttribute("endNarrative")]
    public string endNarrative;

    public Scenario(Scenario[] scenarios)
    {
        ResetHash(scenarios);
    }

    public void ResetHash(Scenario[] scenarios)
    {
        // Using hash of current time to get a number that will hopefully be unique
        var hash = DateTime.Now.GetHashCode();

        // Ensure no other scenario in the passed list has the same id
        // a true flag means that the code hasn't hit another scenario with the same id
        bool flag = false;
        while (!flag)
        {
            flag = true;
            foreach (var actor in scenarios)
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
