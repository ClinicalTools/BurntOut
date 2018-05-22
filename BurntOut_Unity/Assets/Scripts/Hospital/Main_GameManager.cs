﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_GameManager : MonoBehaviour {

    public Interact_Patient room1;
    public Interact_Patient room2;

    // store current patient
    public Interact_Patient currentRoom;

    public GameObject Canvas_Paused;
    public GameObject Canvas_Win;
    public GameObject UI_ChoiceDia;

    //player controller
    public GameObject player;
    

    public bool gamePaused;

    void Start() {

        // hide unnessesary UI
        UI_ChoiceDia.SetActive(false);

        currentRoom = null;
        
        // make game active
        Time.timeScale = 1;

    }

    void Update() {


        // win condition check ---> (in update now, but if performance lacks, make a function that you can call once and check completion)
        if (room1.completed == true && room2.completed == true) {
            Debug.Log("Game Completed");
            Time.timeScale = 0;
            Canvas_Win.SetActive(true);
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        }


        // pause screen 
        if (Input.GetKeyDown(KeyCode.P)) {

            if (gamePaused == true) {
                Debug.Log("Resume Game");
                Time.timeScale = 1;
                gamePaused = false;
                Canvas_Paused.SetActive(false);
                player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

            } else {
                Debug.Log("Game Pause");
                Time.timeScale = 0;
                gamePaused = true;
                Canvas_Paused.SetActive(true);
                player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

            }

        }

	}

    public void ExitRoom() {

        // freeze player controller
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

    }

    public void RoomComplete() {

        currentRoom.completed = true;

    }


}
