using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;

[Serializable]
[XmlRoot("Choice")]
public class Choice
{
    public string name;
    public string text;

    [SerializeField]
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
