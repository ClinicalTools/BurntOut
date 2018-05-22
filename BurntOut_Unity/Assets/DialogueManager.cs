using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // References to other gui objects set in the editor
    public Button[] buttons = new Button[3];
    public Text dialogueText;
    public Text feedbackText;
    public Text promptText;
    public PlayerStats stats;

    private Text[] buttonsText;
    private Scenario scenario;
    private int choiceNum;
    private int optionSelected;
    private Queue<Task> tasks;

    // Initialization
    void Start()
    {
        buttonsText = new Text[buttons.Length];

        choiceNum = 0;
        optionSelected = -1;


        for (int i = 0; i < buttons.Length; i++)
            buttonsText[i] = buttons[i].gameObject.GetComponentInChildren<Text>();
    }

    // Automatically progress through the narrative
    IEnumerator AutoProgress()
    {
        for (; ; )
        {
            yield return new WaitForSeconds(2f);

            ProgressNarrative();
        }
    }

    public void StartScenario(Scenario scenario)
    {
        this.scenario = scenario;
        choiceNum = 0;
        optionSelected = -1;

        tasks = new Queue<Task>(scenario.Choices[choiceNum].Events);

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);

        ProgressNarrative();

        StartCoroutine("AutoProgress");
    }

    public void ProgressNarrative()
    {
        // If no more tasks, show the options or process the end of the displayed option
        if (tasks.Count == 0)
        {
            // No option selected, so prompt the player to pick one
            if (optionSelected < 0)
            {
                dialogueText.gameObject.SetActive(false);
                PromptChoice();
            }
            // Finish processing the selected option
            else
            {
                OptionResults result = scenario.Choices[choiceNum].Options[optionSelected].Result;
                optionSelected = -1;

                switch (result)
                {
                    case OptionResults.CONTINUE:
                        choiceNum++;
                        tasks = new Queue<Task>(scenario.Choices[choiceNum].Events);
                        ProgressNarrative();
                        break;
                    case OptionResults.END:
                        // Not the correct way to end the narrative
                        scenario = null;
                        gameObject.SetActive(false);
                        break;
                    case OptionResults.TRY_AGAIN:
                        tasks = new Queue<Task>(scenario.Choices[choiceNum].Events);
                        ProgressNarrative();
                        break;
                }
            }
        }
        else
        {
            Task task = tasks.Dequeue();

            switch (task.action)
            {
                case TaskAction.TALK:
                    ShowText(scenario.Actors[task.actor], task.dialogue);
                    break;
                case TaskAction.EMOTION:
                    // Show emotion code
                    ProgressNarrative();
                    break;
                case TaskAction.ACTION:
                    // Action code
                    ProgressNarrative();
                    break;
            }
        }

    }

    // Shows a line of dialogue
    // Actor should be used in the future
    public void ShowText(string actor, string dialogue)
    {
        dialogueText.gameObject.SetActive(true);
        dialogueText.text = dialogue;
    }

    // Show the player the options
    private void PromptChoice()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = scenario.Choices[choiceNum].Text;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttonsText[i].text = scenario.Choices[choiceNum].Options[i].Text;
        }
    }

    // React to an option being chosen 
    public void OptionClicked(int option)
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);

        optionSelected = option;
        tasks = new Queue<Task>(scenario.Choices[choiceNum].Options[option].Events);

        stats.currentHealth += scenario.Choices[choiceNum].Options[option].HealthChange;
    }
}
