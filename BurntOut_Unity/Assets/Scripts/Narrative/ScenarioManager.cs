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
        private Scenario scenario = new Scenario(new Scenario[0]);
        public Scenario Scenario
        {
            get
            {
                if (scenario == null)
                    scenario = new Scenario(new Scenario[0]);

                return scenario;
            }
            set
            {
                scenario = value;
            }
        }
    }
}