using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Narrative
{
    public class DialogueManager : MonoBehaviour
    {
        public static DialogueManager Instance { get; private set; }

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
        private bool inChoice, inDialogue;
        private Option option;

        // Queue of tasks (actions, emotions, and dialogue) to perform in current choice or option
        private Queue<Task> tasks = new Queue<Task>();
        private Queue<Task> narrativeTasks;
        private bool inSmallNarrative;


        // Use this for initialization
        private void Awake()
        {
            Instance = this;
        }

        private void Start()
        {
            scenario = ScenarioManager.Instance.Scenario;
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

        // Indicates whether the auto progression should be running
        private void ProgressNarrative()
        {
            if (dialogueTyper.Typing)
            {
                dialogueTyper.FinishTyping();
                return;
            }

            if (tasks.Count == 0 && inSmallNarrative)
            {
                inSmallNarrative = false;
                tasks = narrativeTasks;
                EndDialogue();
                return;
            }
            else if (tasks.Count == 0 && choices[eventSet].isChoice && !inChoice)
            {
                ShowOptions();
            }
            else if (tasks.Count == 0)
            {
                if (inChoice)
                {
                    /*
                    if (option.result == OptionResult.TRY_AGAIN || option.result == OptionResult.END)
                        eventSet--;
                    //*/

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
                            var actorObject = actorObjects.First(a => a.actor.id == task.actorId);
                            if (actorObject != null && task.position != null)
                                task.position.FadeTo(actorObject);
                            break;
                        case TaskAction.Show:
                            if (task.activatable != null)
                                task.activatable.SetActive(true);
                            break;
                        case TaskAction.Hide:
                            if (task.activatable != null)
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
        private readonly Color invisible = new Color(0, 0, 0, 0);
        private void ProcessCharacterDialogue(int actorId, string dialogue)
        {
            foreach (var actor in actorObjects)
                foreach (Transform child in actor.transform)
                    if (child.tag == "Scene")
                        child.gameObject.SetActive(false);
                    else if (child.tag == "Dialogue")
                        child.gameObject.SetActive(actor.actor.id == actorId);
                
            if (actorId == Actor.NARRATOR_ID)
            {
                nameText.text = NARRATOR_NAME;
                if (actorImage.sprite == null)
                    actorImage.color = invisible;
                else
                    actorImage.color =  darkenedCharColor;
            }
            else if (actorId == Actor.PLAYER_ID)
            {
                nameText.text = PLAYER_NAME;
                if (actorImage.sprite == null)
                    actorImage.color = invisible;
                else
                    actorImage.color = darkenedCharColor;
            }
            else
            {
                var actor = actorObjects.FirstOrDefault(a => a.actor.id == actorId)?.actor;
                if (actor != null)
                {
                    actorImage.sprite = actor.neutral;
                    if (actorImage.sprite == null)
                        actorImage.color = invisible;
                    else
                        actorImage.color = Color.white;
                    nameText.text = actor.name;
                }
                else
                {
                    nameText.text = "?";
                    if (actorImage.sprite == null)
                        actorImage.color = invisible;
                    else
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

        private void ShowOptions()
        {
            continueButton.gameObject.SetActive(false);

            dialogueTyper.UpdateText(choices[eventSet].text);
            for (int i = 0; i < choices[eventSet].Options.Count; i++)
            {
                optionButtons[i].gameObject.SetActive(true);
                optionButtonsText[i].text = choices[eventSet].Options[i].text;
            }

            nameText.text = "";
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
                PlayerMovement.Instance.ZoomLook(actorObject.ActorTransform, 2);
                actorImage.sprite = actorObject.actor.neutral;
                StartDialogue();
            }
            else
            {
                var actorEvents = scenario.ActorEventsList.FirstOrDefault(
                    a => a.actorId == actorObject.actor.id);
                if (actorEvents?.Events?.Count > 0)
                {
                    narrativeTasks = tasks;
                    tasks = new Queue<Task>();
                    foreach (var task in actorEvents.Events)
                        tasks.Enqueue(task);
                    inSmallNarrative = true;

                    PlayerMovement.Instance.ZoomLook(actorObject.ActorTransform, 2);
                    actorImage.sprite = actorObject.actor.neutral;
                    StartDialogue();
                }
            }
        }

        public void ObjectInteract(Interactable interactable)
        {
            if (eventSet < choices.Count && choices[eventSet].Triggers.Exists(
                t => t.type == TriggerType.Interact && t.interactable == interactable))
            {
                PlayerMovement.Instance.ZoomLook(interactable.transform, 2);
                actorImage.sprite = null;
                StartDialogue();
            }
            else if (interactable.Events?.Count > 0)
            {
                narrativeTasks = tasks;
                tasks = new Queue<Task>();
                foreach (var task in interactable.Events)
                    tasks.Enqueue(task);
                inSmallNarrative = true;

                PlayerMovement.Instance.ZoomLook(interactable.transform, 2);
                actorImage.sprite = null;
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

            foreach (var actor in actorObjects)
                foreach (Transform child in actor.transform)
                    if (child.tag == "Scene")
                        child.gameObject.SetActive(false);

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

            foreach (var actor in actorObjects)
                foreach (Transform child in actor.transform)
                    if (child.tag == "Scene")
                        child.gameObject.SetActive(true);
                    else if (child.tag == "Dialogue")
                        child.gameObject.SetActive(false);

            myanim.SetBool("End", true);


            StartCoroutine(DisableDiaUI());

            PlayerMovement.Instance.ReturnPosition();
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
    }
}