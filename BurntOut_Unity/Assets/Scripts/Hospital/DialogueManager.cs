using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    // References to other gui objects set in the editor
    public Button[] buttons = new Button[3];
    public Button continueButton;
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
    //private bool runAutoProgress;
    //private Coroutine autoProgress;
    // Queue of tasks (actions, emotions, and dialogue) to perform in current choice or option
    private Queue<Task> tasks;
    public bool InDialogue { get; private set; }

    // Initialize references to button text
    void Start()
    {
        buttonsText = new Text[buttons.Length];
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].onClick.RemoveAllListeners();
            // Creating a new variable to hold option number because the delegate would use the value of i from after the for loop
            var optionNum = i;
            buttons[i].onClick.AddListener(delegate { OptionClicked(optionNum); });
            buttonsText[i] = buttons[i].gameObject.GetComponentInChildren<Text>();
        }
        continueButton.onClick.AddListener(delegate { ProgressNarrative(); });
    }
    
    private void Update()
    {
        //if (InDialogue && Input.GetMouseButtonDown(0))
            //ProgressNarrative();
    }

    // Resets the dialogue manager to be used with a passed scenario
    public void StartScenario(Scenario scenario)
    {
        // If this was already the selected scenario, don't reset variables
        if (this.scenario == scenario)
            return;

        this.scenario = scenario;
        choiceNum = -1;
        optionSelected = -1;
        lost = false;

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

        InDialogue = true;
    }

    /// <summary>
    /// Finish talking with the patient
    /// </summary>
    public void EndDialogue()
    {
        UI_ChoiceDia.SetActive(false);

        InDialogue = false;
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

                ShowFeedback(scenario.endNarrative);
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
                gameManager.RoomLost();
                gameManager.ExitRoom();
            }
            // Otherwise continue with the narrative
            else
            {
                feedback = false;
                feedbackText.transform.parent.gameObject.SetActive(false);
                dialogueText.transform.parent.gameObject.SetActive(true);

                ProgressNarrative();

                if (stats.LowHealth())
                    gameManager.ExitRoom();
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

                switch (option.result)
                {
                    case OptionResult.CONTINUE:
                        choiceNum++;
                        // Load tasks for the next choice
                        if (choiceNum < scenario.Choices.Count)
                            tasks = new Queue<Task>(scenario.Choices[choiceNum].Events);

                        ShowFeedback(option.feedback);
                        break;
                    case OptionResult.END:
                        lost = true;
                        ShowFeedback(option.feedback);
                        break;
                    case OptionResult.TRY_AGAIN:
                        tasks = new Queue<Task>(scenario.Choices[choiceNum].Events);
                        ShowFeedback(option.feedback);
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
                    ShowText(scenario.GetActor(task.actorId).name, task.dialogue);
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
    }

    // Shows a line of dialogue
    // Actor should be used in the future
    private void ShowText(string actorName, string dialogue)
    {
        dialogueText.transform.parent.gameObject.SetActive(true);
        dialogueText.text = dialogue;
    }

    // Show the player the options
    private void PromptChoice()
    {
        promptText.transform.parent.gameObject.SetActive(true);
        promptText.text = scenario.Choices[choiceNum].text;
        for (int i = 0; i < buttons.Length; i++)
        {
            buttons[i].gameObject.SetActive(true);
            buttonsText[i].text = scenario.Choices[choiceNum].Options[i].text;
        }
    }

    // React to an option being chosen 
    private void OptionClicked(int option)
    {
        for (int i = 0; i < buttons.Length; i++)
            buttons[i].gameObject.SetActive(false);

        promptText.transform.parent.gameObject.SetActive(false);
        dialogueText.transform.parent.gameObject.SetActive(true);

        optionSelected = option;

        tasks = new Queue<Task>(scenario.Choices[choiceNum].Options[option].Events);
        
        stats.CurrentHealth += scenario.Choices[choiceNum].Options[option].healthChange;

        ProgressNarrative();
    }
}
