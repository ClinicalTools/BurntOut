using UnityEngine;

public class Interact_Patient : MonoBehaviour
{
    public int patientNumber;
    public bool completed;
    public bool isAroundPatient;

    public GameObject player;

    public Main_GameManager gameManager;
    public DialogueManager dialogueManager;

    public float maxAngle = 35;

    void Update()
    {
        // if player is around patient, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (isAroundPatient == true)
            {
                Vector3 vec = transform.position - player.transform.position;
                vec.y = 0;

                // Measure from the direction opposite the player
                Vector3 playerDir = player.transform.TransformDirection(Vector3.forward);
                playerDir.y = 0;

                if (Vector3.Angle(playerDir, vec) < maxAngle)
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
                }
            }

        }

    }


    // detect if player is within interact range
    void OnTriggerEnter(Collider col)
    {

        if (col.gameObject.name == "Player")
        {

            Debug.Log("Player Detected");
            isAroundPatient = true;

        }
        else
        {

            Debug.Log("Other obj detected");
            isAroundPatient = false;

        }

    }

    // return to default state when out of range
    void OnTriggerExit(Collider col)
    {

        Debug.Log("Player away from patient");
        isAroundPatient = false;

        dialogueManager.EndDialogue();
    }
}
