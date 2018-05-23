using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
[XmlRoot("Scenario")]
public class Scenario
{
    [XmlAttribute("name")]
    public string Name;

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
        for (int i = 0; i <  actors.Count; i++)
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
    public string EndNarrative;
}
