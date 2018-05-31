﻿using UnityEngine;
using UnityEngine.PostProcessing;
using System.Collections;

public class Main_GameManager : MonoBehaviour
{
    private int roomsWon;
    private int roomsLost;

    // store current patient
    public InteractPatient currentRoom;

    public GameObject Canvas_Paused;
    public GameObject Canvas_Win;
    public GameObject Canvas_Loss;

    // UIs
    public GameObject UI_ChoiceDia;
    public GameObject UI_ReadingStation;
    public GameObject UI_MatchingStation;

    // Win UI
    public UI_Star[] Stars;
    public UI_Star[] BadStars;
    public UI_Star Star_RoomA;
    public UI_Star Star_RoomB;
    public UI_Star Star_RoomABAD;
    public UI_Star Star_RoomBBAD;

    //player controller
    public GameObject player;
    public GameObject playerCam;
    public PlayerStats playerStats;
    public PostProcessingBehaviour ppScene;

    DepthOfFieldModel.Settings dofSettings;
    public DialogueManager dialogueManager;

    public bool gameover;
    public bool gamePaused;

    public bool hospitalwin;

    void Start()
    {
        // hide unnessesary UI here
        UI_ChoiceDia.SetActive(false);

        currentRoom = null;

        // make game active
        Time.timeScale = 1;

        playerStats = player.GetComponent<PlayerStats>();

        // post processing settings here
        dofSettings = ppScene.profile.depthOfField.settings;

        // start scene not blured
        ScreenUnblur();

        hospitalwin = false;
    }

    void Update()
    {
        // pause screen 
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (gamePaused == true)
            {
                Debug.Log("Resume Game");
                Time.timeScale = 1;
                gamePaused = false;
                Canvas_Paused.SetActive(false);
                player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

                ScreenUnblur();
            }
            else
            {
                Debug.Log("Game Pause");
                Time.timeScale = 0;
                gamePaused = true;
                Canvas_Paused.SetActive(true);
                player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

                ScreenBlur();
            }
        }
    }



    ///////////////////////////////////////////////////////////////////////////
    //////////////////////////////// MINIGAMES ////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////


    public void MinigameStart()
    {
        ScreenBlur();
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponentInChildren<PlayerRotateToTarget>().enabled = true;
    }

    public void MinigameEnd()
    {
        ScreenUnblur();
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.GetComponentInChildren<PlayerRotateToTarget>().enabled = false;
    }

    ///////////////////////////////////////////////////////////////////////////
    //////////////////////////////// MINIGAMES ////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////



    public void ScreenBlur()
    {
        dofSettings.focusDistance = 0.1f;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

    public void ScreenUnblur()
    {
        dofSettings.focusDistance = 0.94f;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

    public void ExitRoom()
    {
        dialogueManager.EndDialogue();

        // freeze player controller
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

        UI_ChoiceDia.SetActive(false);
        playerCam.GetComponent<PlayerRotateToTarget>().enabled = false;

    }

    public void RoomComplete()
    {
        Stars[roomsWon++].StartAnimation();
        if (roomsWon >= 3)
            hospitalwin = true;

        currentRoom.door.doorlocked = false;
        currentRoom.completed = true;
    }
    public void RoomLost()
    {
        BadStars[roomsLost++].StartAnimation();
        if (roomsLost >= 3)
            Lose();

        currentRoom.door.doorlocked = false;
        currentRoom.lost = true;
    }

    // on loss here
    // l   | l i
    // l l | l -
    public void Lose()
    {
        StartCoroutine(LoseWithDelay());
    }

    public void returnToMainMenu() {
        Application.LoadLevel("MainMenu");
    }

    public IEnumerator LoseWithDelay() {

        yield return new WaitForSeconds(.05f);
        Canvas_Loss.SetActive(true);
        Debug.Log("loss");
        ScreenBlur();
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }
    
    


}
