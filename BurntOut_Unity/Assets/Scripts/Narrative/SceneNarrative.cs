using System;
using System.Collections.Generic;
using UnityEngine;

namespace Narrative
{
    [Serializable]
    public class SceneNarrative
    {
        [SerializeField]
        public List<Scenario> scenarios = new List<Scenario>();
        public int ScenarioIndex(int id)
        {
            for (int i = 0; i < scenarios.Count; i++)
                if (scenarios[i].id == id)
                    return i;

            return -1;
        }
        public Scenario GetScenario(int id)
        {
            foreach (Scenario scenario in scenarios)
                if (scenario.id == id)
                    return scenario;

            return null;
        }
        public string[] ScenarioNames()
        {
            var arr = new string[scenarios.Count];
            for (int i = 0; i < arr.Length; i++)
                arr[i] = scenarios[i].name;

            return arr;
        }

        [SerializeField]
        public string startNarration;
        [SerializeField]
        public string endNarration;
    }
}