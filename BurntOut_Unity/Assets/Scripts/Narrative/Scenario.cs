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
    private List<string> actors;
    public List<string> Actors
    {
        get
        {
            if (actors == null)
                actors = new List<string>();

            return actors;
        }
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
