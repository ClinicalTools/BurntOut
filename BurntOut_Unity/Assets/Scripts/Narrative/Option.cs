using System;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Serialization;
using UnityEngine;

public enum OptionResults
{
    TRY_AGAIN, END, CONTINUE
}

[Serializable]
[XmlRoot("Option")]
public class Option
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

    [XmlAttribute("Result")]
    public OptionResults Result;

    public int HealthChange
    {
        get
        {
            switch (Result)
            {
                case OptionResults.CONTINUE:
                    return 10;
                case OptionResults.TRY_AGAIN:
                    return -30;
                case OptionResults.END:
                    return -60;
                default:
                    Debug.Log("Invalid option result");
                    return 0;
            }
        }
    }

    [XmlAttribute("Feedback")]
    public string Feedback;
}