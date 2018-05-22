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
    // Other references
    public PlayerStats stats;
    public Main_GameManager gameManager;


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
            yield return new WaitForSeconds(1.5f);

            ProgressNarrative();
        }
    }

    Coroutine autoProgress;
    public void StartScenario(Scenario scenario)
    {
        this.scenario = scenario;
        choiceNum = 0;
        optionSelected = -1;

        tasks = new Queue<Task>(scenario.Choices[choiceNum].Events);

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);

        feedbackText.transform.parent.gameObject.SetActive(false);
        promptText.transform.parent.gameObject.SetActive(false);
        dialogueText.transform.parent.gameObject.SetActive(true);

        ProgressNarrative();

        autoProgress = StartCoroutine("AutoProgress");
    }

    private bool feedback;
    public void ProgressNarrative()
    {
        if (feedback)
        {
            // If all choices have been done, show the final narrative
            if (choiceNum == scenario.Choices.Count)
            {
                choiceNum++;

                ShowFeedback(scenario.EndNarrative);
            }
            // If the final narrative has been shown, the room has been completed
            else if (choiceNum >= scenario.Choices.Count)
            {
                gameManager.RoomComplete();
                gameManager.ExitRoom();
            }
            // If the player lost the room because of a bad option
            else if (choiceNum < 0)
            {
                gameManager.ExitRoom();
            }
            // Otherwise continue with the narrative
            else
            {
                feedback = false;
                feedbackText.transform.parent.gameObject.SetActive(false);
                dialogueText.transform.parent.gameObject.SetActive(true);

                ProgressNarrative();
            }
        }
        // If no more tasks, show the options or process the end of the displayed option
        else if (tasks.Count == 0)
        {
            dialogueText.transform.parent.gameObject.SetActive(false);

            // No option selected, so prompt the player to pick one
            if (optionSelected < 0)
            {
                PromptChoice();
            }
            // Finish processing the selected option
            else
            {
                var option = scenario.Choices[choiceNum].Options[optionSelected];
                optionSelected = -1;

                switch (option.Result)
                {
                    case OptionResults.CONTINUE:
                        choiceNum++;
                        // Load tasks for the next choice
                        if (choiceNum < scenario.Choices.Count)
                            tasks = new Queue<Task>(scenario.Choices[choiceNum].Events);

                        ShowFeedback(option.Feedback);
                        break;
                    case OptionResults.END:
                        choiceNum = -1;
                        ShowFeedback(option.Feedback);
                        break;
                    case OptionResults.TRY_AGAIN:
                        tasks = new Queue<Task>(scenario.Choices[choiceNum].Events);
                        ShowFeedback(option.Feedback);
                        break;
                }
            }
        }
        else
        {
            dialogueText.gameObject.SetActive(true);

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

    public void ShowFeedback(string feedback)
    {
        this.feedback = true;

        feedbackText.transform.parent.gameObject.SetActive(true);

        feedbackText.text = feedback;

        Debug.Log("Show feedback: " + feedback);
    }

    // Shows a line of dialogue
    // Actor should be used in the future
    public void ShowText(string actor, string dialogue)
    {
        dialogueText.transform.parent.gameObject.SetActive(true);
        dialogueText.text = dialogue;
    }

    // Show the player the options
    private void PromptChoice()
    {
        StopCoroutine(autoProgress);

        promptText.transform.parent.gameObject.SetActive(true);
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
        autoProgress = StartCoroutine("AutoProgress");

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);
        promptText.transform.parent.gameObject.SetActive(false);
        dialogueText.transform.parent.gameObject.SetActive(true);

        optionSelected = option;
        tasks = new Queue<Task>(scenario.Choices[choiceNum].Options[option].Events);
        
        stats.currentHealth += scenario.Choices[choiceNum].Options[option].HealthChange;

        ProgressNarrative();
    }
}
