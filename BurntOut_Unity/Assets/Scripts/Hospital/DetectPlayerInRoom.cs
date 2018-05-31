using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DetectPlayerInRoom : MonoBehaviour {

    public bool isPlayerInRoom;

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Player")
            isPlayerInRoom = true;

    }

    // return to default state when out of range
    void OnTriggerExit(Collider col) {
        if (col.gameObject.name == "Player") {
            isPlayerInRoom = false;
        }
    }


}
