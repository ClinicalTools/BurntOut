using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DoorInteract : MonoBehaviour {

    public bool isAroundStation;
    public bool playerFacing;

    public Text interactPrompt;
    public GameObject player;
    public PlayerStats stats;

    public Animator myanimator;
    public int doorVariable = 0;
    public GameObject doorHandle;

    public DetectPlayerInRoom detectPlayerinRoom;
    public InteractPatient patient;
    public bool doorlocked;

    public float maxAngle = 35;

    void Update() {

        if (isAroundStation) {
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
        }

        // if player is around patient, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E) && isAroundStation && playerFacing && !stats.LowHealth()) {
            interactPrompt.transform.parent.gameObject.SetActive(false);

            // INTERACTION HERE

            // if open door the first time...
            if (doorVariable == 0) {
                myanimator.SetInteger("dooranim", 1);
                doorVariable = 1;
            }

            // if open close
            else if (doorVariable == 1) {
                myanimator.SetInteger("dooranim", -1);
                doorVariable = -1;

                // let patient know that you can begin
                if (detectPlayerinRoom.isPlayerInRoom) {
                    patient.doorClosedInBounds = true;

                    // lock room if room not completed
                    if (!patient.completed)
                    doorlocked = true;
                }
                


            }

            // if close open
            else if (doorVariable == -1) {

                if (!doorlocked) {
                    myanimator.SetInteger("dooranim", 1);
                    doorVariable = 1;
                }
            }

            StartCoroutine(Rotate());
        }
    }

    public IEnumerator Rotate() {

        player.GetComponentInChildren<PlayerRotateToTarget>().target = doorHandle;
        player.GetComponentInChildren<PlayerRotateToTarget>().enabled = true;
        yield return new WaitForSeconds(1);
        player.GetComponentInChildren<PlayerRotateToTarget>().enabled = false;

    }

    private void Look() {

        interactPrompt.transform.parent.gameObject.SetActive(true);

        if (stats.LowHealth())
            interactPrompt.text = "Energy too low to open door";
        else if (doorVariable == 0 || doorVariable == -1) {

            if (doorlocked)
                interactPrompt.text = "DOOR LOCKED!";
            else
                interactPrompt.text = "Press 'e' to open door";

        } else if (doorVariable == 1)
            interactPrompt.text = "Press 'e' to close door";
    }

    private void LookAway() {
        interactPrompt.transform.parent.gameObject.SetActive(false);
    }

    // detect if player is within interact range
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Player")
            isAroundStation = true;
    }

    // return to default state when out of range
    void OnTriggerExit(Collider col) {
        if (col.gameObject.name == "Player") {
            isAroundStation = false;
            playerFacing = false;
            LookAway();
        }
    }
}
