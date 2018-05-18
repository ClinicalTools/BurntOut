using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_GameManager : MonoBehaviour {

    public InteractableDoor room1;
    public InteractableDoor room2;

    void Start () {

 

    }

    void Update () {

        if (room1.completed == true && room2.completed == true) {
            Debug.Log("Game Completed");
        }

	}


}
