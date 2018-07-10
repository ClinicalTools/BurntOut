using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Narrative
{
    public class ScenarioDialogueManager : MonoBehaviour
    {
        public static ScenarioDialogueManager Instance { get; private set; }

        private const string PLAYER_NAME = "Player";
        private const string NARRATOR_NAME = "NARRATOR";

        // References to other objects set in the editor
        public Button[] optionButtons = new Button[3];
        public Button continueButton;
        public Text nameText;
        public TextTyper dialogueTyper;
        public Image actorImage;
        public GameObject DialogueUI;

        public Animator myanim;

        private Text[] optionButtonsText;
        private ActorObject[] actorObjects;

        // Data used to keep track of place in the dialogue 
        private Scenario scenario;
        private List<Choice> choices;
        private int eventSet = -1;
        private bool inChoice, inDialogue = false;
        private Option option;
        
        // Use this for initialization
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            scenario = ScenarioManager.Instance.Scenario;
            choices = scenario.Choices;

            actorObjects = FindObjectsOfType<ActorObject>();

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

        // Indicates whether the auto progression should be running
        // Queue of tasks (actions, emotions, and dialogue) to perform in current choice or option
        private readonly Queue<Task> tasks = new Queue<Task>();
        private void ProgressNarrative()
        {
            if (dialogueTyper.Typing)
            {
                dialogueTyper.FinishTyping();
                return;
            }

            if (tasks.Count == 0 && choices[eventSet].isChoice && !inChoice)
            {
                ShowOptions();
            }
            else if (tasks.Count == 0)
            {
                if (inChoice)
                {
                    if (option.result == OptionResult.TRY_AGAIN || option.result == OptionResult.END)
                        eventSet--;

                    if (!string.IsNullOrEmpty(option.feedback))
                        Main_GameManager.Instance.FeedbackTyper.UpdateText(option.feedback);

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
            switch (task.type)
            {
                case TaskType.Action:
                    switch (task.action)
                    {
                        case TaskAction.MoveTo:
                            task.position.FadeTo();
                            break;
                        case TaskAction.Show:
                            task.activatable.SetActive(true);
                            break;
                        case TaskAction.Hide:
                            task.activatable.SetActive(false);
                            break;
                        case TaskAction.Script:
                            break;
                    }
                    ProgressNarrative();
                    break;
                case TaskType.Emotion:
                    ProcessCharacterEmotion(task.actorId, task.emotion);
                    break;
                case TaskType.Talk:
                    ProcessCharacterDialogue(task.actorId, task.dialogue);
                    break;
            }
        }

        private readonly Color darkenedCharColor = new Color(.8f, .8f, .8f);
        private void ProcessCharacterDialogue(int actorId, string dialogue)
        {
            if (actorId == Actor.NARRATOR_ID)
            {
                nameText.text = NARRATOR_NAME;
                actorImage.color = darkenedCharColor;
            }
            else if (actorId == Actor.PLAYER_ID)
            {
                nameText.text = PLAYER_NAME;
                actorImage.color = darkenedCharColor;
            }
            else
            {
                var actor = actorObjects.FirstOrDefault(a => a.actor.id == actorId)?.actor;
                if (actor != null)
                {
                    actorImage.sprite = actor.neutral;
                    actorImage.color = Color.white;
                    nameText.text = actor.name;
                }
                else
                {
                    nameText.text = "?";
                    actorImage.color = darkenedCharColor;
                }
            }

            dialogueTyper.UpdateText(dialogue);
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

            foreach (var optionBtn in optionButtons)
                optionBtn.gameObject.SetActive(false);

            option = choices[eventSet].Options[optionNum];
            foreach (var task in option.Events)
                tasks.Enqueue(task);
            
            ProgressNarrative();
        }

        public void ActorInteract(ActorObject actorObject)
        {
            if (eventSet < choices.Count && choices[eventSet].Triggers.Exists(
                t => t.type == TriggerType.Talk && t.id == actorObject.actor.id))
            {
                PlayerRotateToTarget.Instance.ZoomLook(actorObject.gameObject, 2);
                StartDialogue();
            }
        }

        private void StartDialogue()
        {
            continueButton.gameObject.SetActive(true);

            inDialogue = true;

            foreach (var actorObject in actorObjects)
                actorObject.Hide();

            Main_GameManager.Instance.ScreenBlur();
            Main_GameManager.Instance.isCurrentlyExamine = true;

            DialogueUI.SetActive(true);
            dialogueTyper.Wait = true;
            ProgressNarrative();
            myanim.SetTrigger("DialogueStart");
        }

        private void EndDialogue()
        {
            continueButton.gameObject.SetActive(false);

            if (eventSet >= choices.Count && scenario.sceneChange && scenario.autoChangeScene)
                ChangeScenes();

            inDialogue = false;

            foreach (var actorObject in actorObjects)
                actorObject.Show();

            Main_GameManager.Instance.ScreenUnblur();
            myanim.SetBool("End", true);


            StartCoroutine(DisableDiaUI());

            PlayerRotateToTarget.Instance.ReturnPosition();
        }

        public IEnumerator DisableDiaUI()
        {

            yield return new WaitForSeconds(1f);
            DialogueUI.SetActive(false);

            //myanim.SetBool("End", false);
            Main_GameManager.Instance.isCurrentlyExamine = false;
        }

        private void ChangeScenes()
        {
            Main_GameManager.Instance.Transition(scenario.scenePath);
        }

        private void ShowOptions()
        {
            continueButton.gameObject.SetActive(false);

            dialogueTyper.UpdateText(choices[eventSet].text);
            for (int i = 0; i < choices[eventSet].Options.Count; i++)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtonsText[i].text = choices[eventSet].Options[i].text;
            }
        }
    }
}