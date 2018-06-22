using UnityEngine;

[ExecuteInEditMode]
public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Instance { get; private set; }

    [SerializeField]
    public Scenario scenario = new Scenario(new Scenario[0]);

    private void OnEnable()
    {
        Instance = this;

        // Ensure there's at least one scenario
        if (scenario == null)
            scenario = new Scenario(new Scenario[0]);
    }
}
