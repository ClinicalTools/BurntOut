using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Narrative
{
    public class ScenarioDialogueManager : MonoBehaviour
    {
        public static ScenarioDialogueManager Instance { get; private set; }

        // References to other gui objects set in the editor
        public Button[] optionButtons = new Button[3];
        public Button continueButton;
        public Text nameText;
        public Text dialogueText;
        public Text feedbackText;
        public Text promptText;
        public GameObject DialogueUI;

        private Text[] optionButtonsText;
        private ActorObject[] actorObjects;

        // Use this for initialization
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            scenario = ScenarioManager.Instance.scenario;
            choices = scenario.Choices;

            optionButtonsText = new Text[optionButtons.Length];
            for (int i = 0; i < optionButtons.Length; i++)
            {
                optionButtons[i].onClick.RemoveAllListeners();
                // Creating a new variable to hold option number because the delegate would use the value of i from after the for loop
                var optionNum = i;
                optionButtons[i].onClick.AddListener(delegate { OptionClicked(optionNum); });
                optionButtonsText[i] = optionButtons[i].gameObject.GetComponentInChildren<Text>();
            }
            continueButton.onClick.AddListener(delegate { ProgressNarrative(); });


            actorObjects = FindObjectsOfType<ActorObject>();

            QueueNextEvents();

            DialogueUI.SetActive(false);
        }

        private Scenario scenario;
        private List<Choice> choices;
        private int eventSet = -1;
        private bool inChoice, inDialogue = false;
        private OptionResult optionResult;
        // Indicates whether the auto progression should be running
        // Queue of tasks (actions, emotions, and dialogue) to perform in current choice or option
        private readonly Queue<Task> tasks = new Queue<Task>();
        private void ProgressNarrative()
        {
            if (tasks.Count == 0 && choices[eventSet].isChoice && !inChoice)
            {
                ShowOptions();
            }
            else if (tasks.Count == 0)
            {
                if (inChoice)
                {
                    if (optionResult == OptionResult.END)
                    {
                        EndDialogue();
                        eventSet = choices.Count;
                        return;
                    }

                    if (optionResult == OptionResult.TRY_AGAIN)
                        eventSet--;

                    inChoice = false;
                }

                QueueNextEvents();
            }
            else
            {
                ProcessTask(tasks.Dequeue());
            }
        }

        private void ProcessTask(Task task)
        {
            switch (task.action)
            {
                case TaskAction.ACTION:
                    ProgressNarrative();
                    break;
                case TaskAction.EMOTION:
                    ProcessCharacterEmotion(task.actorId, task.emotion);
                    break;
                case TaskAction.TALK:
                    ProcessCharacterDialogue(task.actorId, task.dialogue);
                    break;
            }
        }

        private void ProcessCharacterDialogue(int actorId, string dialogue)
        {
            var actor = actorObjects.FirstOrDefault(a => a.actor.id == actorId)?.actor;
            if (actor != null)
                nameText.text = actor.name;
            else
                nameText.text = "player";

            dialogueText.text = dialogue;
        }

        private void ProcessCharacterEmotion(int actorId, TaskEmotion emotion)
        {
            ProgressNarrative();
        }

        private void QueueNextEvents()
        {
            eventSet++;
            if (eventSet == choices.Count)
            {
                EndDialogue();
                return;
            }

            foreach (var task in choices[eventSet].Events)
                tasks.Enqueue(task);

            if (inDialogue)
            {
                if (!choices[eventSet].continueLast)
                    EndDialogue();
                else
                    ProgressNarrative();
            }
        }

        private void OptionClicked(int optionNum)
        {
            inChoice = true;
            continueButton.gameObject.SetActive(true);

            promptText.transform.parent.gameObject.SetActive(false);
            foreach (var optionBtn in optionButtons)
                optionBtn.gameObject.SetActive(false);

            foreach (var task in choices[eventSet].Options[optionNum].Events)
                tasks.Enqueue(task);

            dialogueText.transform.parent.gameObject.SetActive(true);

            optionResult = choices[eventSet].Options[optionNum].result;
            ProgressNarrative();
        }

        public void ActorInteract(ActorObject actorObject)
        {
            Debug.Log(choices.Count);

            if (eventSet < choices.Count)
                StartDialogue();
        }

        private void StartDialogue()
        {
            inDialogue = true;

            Debug.Log("starting dialogue");

            Main_GameManager.Instance.ScreenBlur();
            Main_GameManager.Instance.isCurrentlyExamine = true;

            DialogueUI.SetActive(true);
            ProgressNarrative();
        }

        private void EndDialogue()
        {
            inDialogue = false;

            Main_GameManager.Instance.ScreenUnblur();
            Main_GameManager.Instance.isCurrentlyExamine = false;

            DialogueUI.SetActive(false);
        }

        private void ShowOptions()
        {
            continueButton.gameObject.SetActive(false);
            dialogueText.transform.parent.gameObject.SetActive(false);

            promptText.transform.parent.gameObject.SetActive(true);
            promptText.text = choices[eventSet].text;
            for (int i = 0; i < choices[eventSet].Options.Count; i++)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtonsText[i].text = choices[eventSet].Options[i].text;
            }
        }
    }
}