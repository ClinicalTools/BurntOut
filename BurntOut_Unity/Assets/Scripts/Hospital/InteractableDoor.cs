using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : MonoBehaviour {

    public int doorNumber;
    public bool completed;
    public bool isAroundDoor;

    public GameObject UI_ChoiceDia;

    void Update() {

        // if player is around door, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E)) {

            if (isAroundDoor == true) {

                // INTERACTION HERE
                UI_ChoiceDia.SetActive(true);

                Debug.Log("Door open");
                completed = true;






















            }

        }

    }


    // detect if player is within interact range
    void OnTriggerEnter (Collider col) {

        if (col.gameObject.name == "Player") {

            Debug.Log("Player Detected");
            isAroundDoor = true;
            
        } else {

            Debug.Log("Other obj detected");
            isAroundDoor = false;

        }

    } 

    // return to default state when out of range
    void OnTriggerExit (Collider col) {

        Debug.Log("Player away from door");
        isAroundDoor = false;

        UI_ChoiceDia.SetActive(false);

    }
}
