using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact_Patient : MonoBehaviour {

    public int patientNumber;
    public bool completed;
    public bool isAroundPatient;

    public GameObject UI_ChoiceDia;
    public GameObject player;

    public Main_GameManager gameManager;

    void Update() {

        // if player is around patient, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E)) {

            if (isAroundPatient == true) {

                // INTERACTION HERE
                UI_ChoiceDia.SetActive(true);
                UI_ChoiceDia.GetComponent<DialogueManager>().StartScenario(
                GameObject.Find("NarrativeManager").GetComponent<NarrativeManager>().sceneNarrative.scenarios[0]);

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


    // detect if player is within interact range
    void OnTriggerEnter (Collider col) {

        if (col.gameObject.name == "Player") {

            Debug.Log("Player Detected");
            isAroundPatient = true;
            
        } else {

            Debug.Log("Other obj detected");
            isAroundPatient = false;

        }

    } 

    // return to default state when out of range
    void OnTriggerExit (Collider col) {

        Debug.Log("Player away from patient");
        isAroundPatient = false;

        UI_ChoiceDia.SetActive(false);

    }
}
