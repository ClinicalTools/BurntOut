using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
[XmlRoot("Choice")]
public class Choice
{
    [XmlAttribute("name")]
    public string Name;

    [XmlAttribute("text")]
    public string Text;

    [SerializeField]
    [XmlArray("Events"), XmlArrayItem("Event")]
    private List<Task> events;
    public List<Task> Events
    {
        get
        {
            if (events == null)
                events = new List<Task>();

            return events;
        }
    }

    [SerializeField]
    [XmlArray("Options"), XmlArrayItem("Option")]
    private List<Option> options;
    public List<Option> Options
    {
        get
        {
            if (options == null)
                options = new List<Option>();

            return options;
        }    
    }

}
