using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_GameManager : MonoBehaviour {

    public InteractableDoor room1;
    public InteractableDoor room2;

    public GameObject Canvas_Paused;

    public bool gamePaused;

    void Start () {

 

    }

    void Update() {

        if (room1.completed == true && room2.completed == true) {
            Debug.Log("Game Completed");
        }

        if (Input.GetKeyDown(KeyCode.P)) {

            if (gamePaused == true) {
                Debug.Log("Resume Game");
                Time.timeScale = 1;
                gamePaused = false;
                Canvas_Paused.SetActive(false);
            } else {
                Debug.Log("Game Pause");
                Time.timeScale = 0;
                gamePaused = true;
                Canvas_Paused.SetActive(true);
            }

        }

	}


}
