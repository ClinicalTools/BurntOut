using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractableDoor : MonoBehaviour {

    
    public int doorNumber;
    public bool completed;
    public bool isAroundDoor;


    void Update() {

        if (Input.GetKeyDown(KeyCode.E)) {

            if (isAroundDoor == true) {
                Debug.Log("Door open");
                completed = true;

            }

        }

    }

    void OnTriggerEnter (Collider col) {

        if (col.gameObject.name == "Player") {

            Debug.Log("Player Detected");
            isAroundDoor = true;
            //completed = true;

        } else {

            Debug.Log("Other obj detected");
            isAroundDoor = false;

        }

    } 

    void OnTriggerExit (Collider col) {

        Debug.Log("Player away from door");
        isAroundDoor = false;

    }
}
