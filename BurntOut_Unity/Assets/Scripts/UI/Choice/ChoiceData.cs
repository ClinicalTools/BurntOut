using UnityEngine;
using UnityEngine.UI;

public class ChoiceData : MonoBehaviour {

    public string text;
    public float healthResult;
    public int scenario;
    public OptionResults result;
    public int optionNum;
    public static int choiceNum;

    public PlayerStats stats;
    private NarrativeManager narrativeManager;

    private Text textComponent;

    public void Start() {

        //init
        choiceNum = 0;

        textComponent = GetComponentInChildren<Text>();
        narrativeManager = GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>();

        UpdateButton();

    }

    public void ApplyResult() {

        stats.currentHealth += healthResult;

    }

    public void UpdateButton() {
    
        textComponent.text = narrativeManager.sceneNarrative.scenarios[scenario].Choices[choiceNum].Options[optionNum].Text;
        healthResult = narrativeManager.sceneNarrative.scenarios[scenario].Choices[choiceNum].Options[optionNum].HealthChange;

    }

    public void SelectButton() {

        ApplyResult();

    }
}
