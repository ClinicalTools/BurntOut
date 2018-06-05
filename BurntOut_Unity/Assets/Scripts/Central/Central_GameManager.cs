using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

public class Central_GameManager : MonoBehaviour {

    public GameObject Canvas_Paused;

    //player controller
    public GameObject player;
    public GameObject playerCam;
    public PlayerStats playerStats;
    public PostProcessingBehaviour ppScene;

    DepthOfFieldModel.Settings dofSettings;

    public bool gameover;
    public bool gamePaused;

    public GlobalStats globalStats;

    private void Awake() {
        // LOAD DATA HERE
        globalStats = GameObject.FindObjectOfType<GlobalStats>();
        playerStats.CurrentHealth = globalStats.currentHealth;
    }

    void Start() {

        // make game active
        Time.timeScale = 1;

        playerStats = player.GetComponent<PlayerStats>();

        // post processing settings here
        dofSettings = ppScene.profile.depthOfField.settings;

        // start scene not blured
        ScreenUnblur();


    }

    void Update() {
        // pause screen 
        if (Input.GetKeyDown(KeyCode.P)) {
            if (gamePaused == true) {
                Debug.Log("Resume Game");
                Time.timeScale = 1;
                gamePaused = false;
                Canvas_Paused.SetActive(false);
                player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;

                ScreenUnblur();
            } else {
                Debug.Log("Game Pause");
                Time.timeScale = 0;
                gamePaused = true;
                Canvas_Paused.SetActive(true);
                player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

                ScreenBlur();
            }
        }
    }

    private void OnDisable() {
        globalStats.currentHealth = playerStats.CurrentHealth;
    }
        
    public void ScreenBlur() {
        dofSettings.focusDistance = 0.1f;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

    public void ScreenUnblur() {
        dofSettings.focusDistance = 0.94f;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

}
