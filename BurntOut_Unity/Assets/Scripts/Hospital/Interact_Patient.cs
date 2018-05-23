using UnityEngine;
using UnityEngine.UI;

public class Interact_Patient : MonoBehaviour
{
    public int patientId;
    public bool completed;
    public bool isAroundPatient;
    public bool playerFacing;

    public Text pressButton; 
    public GameObject player;

    public Main_GameManager gameManager;
    public DialogueManager dialogueManager;
    public PlayerRotateToTarget playerRotateToTarget;

    public float maxAngle = 35;

    void Update()
    {
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
                dialogueManager.EndDialogue();
        }

        // if player is around patient, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E) && isAroundPatient && playerFacing)
        {
            // INTERACTION HERE
            dialogueManager.StartDialogue();

            Debug.Log("Patient interact");

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
        var scenarioId = GetComponentInParent<Room>().scenarioId;
    }

    private void LookAway()
    {
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
