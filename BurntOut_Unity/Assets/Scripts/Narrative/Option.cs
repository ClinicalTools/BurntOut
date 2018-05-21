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

    [XmlAttribute("HealthChange")]
    public int HealthChange;
    public string HealthChangeStr
    {
        get
        {
            string str = "";
            if (HealthChange > 0)
                str += '+';
            str += HealthChange;
            str += '%';

            return str;
        }
        set
        {
            int oldHealthChange = HealthChange;
            value = value.Replace("%", "");
            if (!Int32.TryParse(value, out HealthChange))
                HealthChange = oldHealthChange;
        }
    }

    [XmlAttribute("Feedback")]
    public string Feedback;
}