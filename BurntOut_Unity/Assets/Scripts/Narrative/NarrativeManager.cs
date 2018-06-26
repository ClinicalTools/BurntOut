using UnityEngine;

namespace Narrative
{
    [ExecuteInEditMode]
    public class NarrativeManager : MonoBehaviour
    {
        public static NarrativeManager Instance { get; private set; }

        private void OnEnable()
        {
            Instance = this;

            // Ensure there's at least one scenario
            if (sceneNarrative.scenarios.Count == 0)
            {
                Scenario scenario = new Scenario(new Scenario[0]);
                sceneNarrative.scenarios.Add(scenario);
            }

        }

        [SerializeField]
        public SceneNarrative sceneNarrative = new SceneNarrative();
    }
}