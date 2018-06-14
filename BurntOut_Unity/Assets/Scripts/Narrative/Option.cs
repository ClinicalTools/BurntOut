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

    public int HealthChange
    {
        get
        {
            switch (result)
            {
                case OptionResult.CONTINUE:
                    return 10;
                case OptionResult.TRY_AGAIN:
                    return -30;
                case OptionResult.END:
                    return -60;
                default:
                    Debug.Log("Invalid option result");
                    return 0;
            }
        }
    }

    public string feedback;
}