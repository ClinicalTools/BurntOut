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
    public GameObject UI_ChoiceDia;
    
    // Other references
    public PlayerStats stats;
    public Main_GameManager gameManager;

    
    private Text[] buttonsText;

    private Scenario scenario;
    // -1 if dialogue hasn't started. 
    // choice.Count if last correct option just finished
    // choice.Count+1 if final narrative has been displayed
    private int choiceNum;
    // Option selected. 0, 1, or 2.
    private int optionSelected;

    // True if the player finished an option that made them lose the room
    private bool lost;

    // Indicates whether the auto progression should be running
    private bool runAutoProgress;
    private Coroutine autoProgress;
    // Queue of tasks (actions, emotions, and dialogue) to perform in current choice or option
    private Queue<Task> tasks;


    // Initialize references to button text
    void Start()
    {
        buttonsText = new Text[buttons.Length];
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

    // Resets the dialogue manager to be used with a passed scenario
    public void StartScenario(Scenario scenario)
    {
        this.scenario = scenario;
        choiceNum = -1;
        optionSelected = -1;
        runAutoProgress = true;

        tasks = new Queue<Task>(scenario.Choices[0].Events);

        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);

        feedbackText.transform.parent.gameObject.SetActive(false);
        promptText.transform.parent.gameObject.SetActive(false);
        dialogueText.transform.parent.gameObject.SetActive(true);

        UI_ChoiceDia.SetActive(false);
    }

    /// <summary>
    /// Start talking with the patient
    /// </summary>
    public void StartDialogue()
    {
        UI_ChoiceDia.SetActive(true);

        // If dialogue hasn't started yet, start it
        if (choiceNum < 0)
        {
            choiceNum = 0;
            ProgressNarrative();
        }

        if (runAutoProgress)
            autoProgress = StartCoroutine(AutoProgress());
    }

    /// <summary>
    /// Finish talking with the patient
    /// </summary>
    public void EndDialogue()
    {
        UI_ChoiceDia.SetActive(false);

        if (autoProgress != null && runAutoProgress)
            StopCoroutine(autoProgress);
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
            else if (lost)
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
                        lost = true;
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

    // Shows feedback after an option or for the final narrative
    private void ShowFeedback(string feedback)
    {
        this.feedback = true;

        feedbackText.transform.parent.gameObject.SetActive(true);

        feedbackText.text = feedback;

        Debug.Log("Show feedback: " + feedback);
    }

    // Shows a line of dialogue
    // Actor should be used in the future
    private void ShowText(string actor, string dialogue)
    {
        dialogueText.transform.parent.gameObject.SetActive(true);
        dialogueText.text = dialogue;
    }

    // Show the player the options
    private void PromptChoice()
    {
        runAutoProgress = false;
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
        runAutoProgress = true;
        autoProgress = StartCoroutine(AutoProgress());

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
