using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Main_GameManager : MonoBehaviour
{

    public InteractPatient room1;
    public InteractPatient room2;

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
    public UI_Star Star_RoomA;
    public UI_Star Star_RoomB;

    //player controller
    public GameObject player;
    public PlayerStats playerStats;
    public PostProcessingBehaviour ppScene;

    DepthOfFieldModel.Settings dofSettings;

    public DialogueManager dialogueManager;

    public bool gameover;
    public bool gamePaused;

    void Start()
    {
        // hide unnessesary UI
        UI_ChoiceDia.SetActive(false);

        currentRoom = null;

        // make game active
        Time.timeScale = 1;

        playerStats = player.GetComponent<PlayerStats>();
        
        // post processing settings here
        dofSettings = ppScene.profile.depthOfField.settings;

        // start scene not blured
        ScreenUnblur();

    }

    void Update()
    {

        if (room1.completed == true) {
            Star_RoomA.StartAnimation();
        }

        if (room2.completed == true) {
            Star_RoomB.StartAnimation();
        }






        // win condition check ---> (in update now, but if performance lacks, make a function that you can call once and check completion)
        if (room1.completed == true && room2.completed == true)
        {
            Debug.Log("Game Completed");
            //Time.timeScale = 0;
            Canvas_Win.SetActive(true);
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        }

        // lose functionality HERE
        if (playerStats.CurrentHealth <= 0)
        {

            // call use lose once 
            if (gameover == false)
            {
                Lose();
                gameover = true;
            }

        }


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

    
    public void MinigameStart() {

        ScreenBlur();
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        player.GetComponent<PlayerRotateToTarget>().enabled = true;
    }

    public void MinigameEnd() {
        ScreenUnblur();
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.GetComponent<PlayerRotateToTarget>().enabled = false;
    }

    ///////////////////////////////////////////////////////////////////////////
    //////////////////////////////// MINIGAMES ////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////



    public void ScreenBlur() {
        dofSettings.focusDistance = 0.1f;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

    public void ScreenUnblur() {
        dofSettings.focusDistance = 0.94f;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

    public void ExitRoom()
    {
        dialogueManager.EndDialogue();

        // freeze player controller
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

        // disable Choice UI
        UI_ChoiceDia.SetActive(false);
    }

    public void RoomComplete()
    {
        currentRoom.completed = true;
    }
    public void RoomLost()
    {
        currentRoom.lost = true;
    }

    // on loss here
    public void Lose()
    {
        Canvas_Loss.SetActive(true);
        Debug.Log("loss");
    }



}
