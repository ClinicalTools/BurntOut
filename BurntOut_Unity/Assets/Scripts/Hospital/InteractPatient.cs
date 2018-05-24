using UnityEngine;
using UnityEngine.UI;

public class InteractPatient : MonoBehaviour
{
    public int patientId;
    public bool completed;
    public bool lost;
    public bool isAroundPatient;
    public bool playerFacing;

    public Text interactPrompt; 
    public GameObject player;

    public Main_GameManager gameManager;
    public DialogueManager dialogueManager;
    public PlayerRotateToTarget playerRotateToTarget;

    public float maxAngle = 35;

    void Update()
    {
        if (completed || lost)
            return;

        if (isAroundPatient)
        {
            Vector3 vec = transform.position - player.transform.position;
            vec.y = 0;

            // Measure from the direction opposite the player
            Vector3 playerDir = player.transform.TransformDirection(Vector3.forward);
            playerDir.y = 0;

            bool wasFacing = playerFacing;
            playerFacing = Vector3.Angle(playerDir, vec) < maxAngle;

            if (wasFacing && !playerFacing)
                LookAway();
            else if (!wasFacing && playerFacing)
                Look();
            else if (playerFacing)
                interactPrompt.transform.parent.gameObject.SetActive(!dialogueManager.InDialogue);

        }

        // if player is around patient, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E) && isAroundPatient && playerFacing)
        {
            interactPrompt.transform.parent.gameObject.SetActive(false);

            // INTERACTION HERE
            dialogueManager.StartDialogue();

            // freeze player controller
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

            // enable mouse
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // give the game manager the "name" of the patient
            gameManager.currentRoom = this;

            // give target to "player rotate to target"
            playerRotateToTarget.target = transform.gameObject;

            // enable playerrotate
            playerRotateToTarget.enabled = true;
        }
    }
    
    private void Look()
    {
        var sceneNarrative = GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().sceneNarrative;
        var scenario = sceneNarrative.GetScenario(GetComponentInParent<Room>().scenarioId);

        interactPrompt.transform.parent.gameObject.SetActive(true);
        interactPrompt.text = "Press 'e' to talk to " + scenario.GetActor(patientId).name;
    }

    private void LookAway()
    {
        interactPrompt.transform.parent.gameObject.SetActive(false);
        dialogueManager.EndDialogue();
    }

    // detect if player is within interact range
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
            isAroundPatient = true;

    }

    // return to default state when out of range
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
            isAroundPatient = false;

        LookAway();
    }
}
