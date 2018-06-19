using System;
using System.Collections.Generic;
using UnityEngine;

public enum OptionResult
{
    TRY_AGAIN, END, CONTINUE
}

[Serializable]
public class Option
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

    public OptionResult result;

    public int healthChange;
    public string HealthChangeStr
    {
        get
        {
            string str = "";
            if (healthChange > 0)
                str += '+';
            str += healthChange;
            str += '%';

            return str;
        }
        set
        {
            int oldHealthChange = healthChange;
            value = value.Replace("%", "");
            if (!Int32.TryParse(value, out healthChange))
                healthChange = oldHealthChange;
        }
    }

    public string feedback;
}