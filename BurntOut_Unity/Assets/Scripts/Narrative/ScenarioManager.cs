using UnityEngine;

namespace Narrative
{
    public class ScenarioManager : MonoBehaviour
    {
        public static ScenarioManager Instance { get; private set; }

        private void Awake()
        {
            Instance = this;
        }

        [SerializeField]
        private Scenario scenario = new Scenario();
        public Scenario Scenario
        {
            get
            {
                if (scenario == null)
                    scenario = new Scenario();

                return scenario;
            }
            set
            {
                scenario = value;
            }
        }
    }
}