using UnityEngine;

namespace Narrative
{
    public class ScenarioManager : MonoBehaviour
    {
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