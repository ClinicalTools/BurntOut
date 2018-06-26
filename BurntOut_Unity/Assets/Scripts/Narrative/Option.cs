using System;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
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
                if (Int32.TryParse(value, out healthChange))
                    healthChange = Mathf.Clamp(healthChange, -100, 100);
                else
                    healthChange = oldHealthChange;
            }
        }

        public bool ValidHealthChange
        {
            get
            {
                switch (result)
                {
                    case OptionResult.CONTINUE:
                        return healthChange <= 100 && healthChange >= 0;
                    case OptionResult.TRY_AGAIN:
                        return healthChange <= 0 && healthChange >= -40;
                    case OptionResult.END:
                        return healthChange <= -20 && healthChange >= -100;
                    default:
                        return false;
                }
            }
        }

        public string feedback;
    }
}