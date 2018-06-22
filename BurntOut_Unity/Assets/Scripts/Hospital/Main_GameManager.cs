using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using System.Collections;

public class Main_GameManager : MonoBehaviour
{
    private int roomsWon;
    private int roomsLost;

    // store current patient
    public InteractActor currentRoom;

    public Animator screenfade;

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

    public GlobalStats globalStats;

    // for examine objects
    public bool isCurrentlyExamine;

    public Scene scene;

    private void Awake()
    {
        // LOAD DATA HERE
        globalStats = FindObjectOfType<GlobalStats>();
        playerStats.CurrentHealth = globalStats.currentHealth;

        Cursor.visible = true;
    }

    private void Start()
    {
        roomsWon = globalStats.GOOD_stars;
        roomsLost = globalStats.BAD_stars;

        StartCoroutine(UpdateStars());

        scene = SceneManager.GetActiveScene();

        //if (scene.name == "Hospital_Patient_SingleRoom")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (scene.name == "ICU_New")
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }

        if (scene.name == "Hospital" || scene.name == "Central")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        }

        if (scene.name == "VitalitySpace")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        }

        // hide unnessesary UI here
        if (scene.name != "ICU_New")
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

                if (scene.name == "VitalitySpace" || scene.name == "Hospital" || scene.name == "Central")
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

    private void OnDisable()
    {
        globalStats.currentHealth = playerStats.CurrentHealth;
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
        dofSettings.aperture = 14;
        dofSettings.focalLength = 25;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

    public void ScreenUnblur()
    {
        dofSettings.focusDistance = 0.94f;
        dofSettings.aperture = 5;
        dofSettings.focalLength = 50;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

    public void ExitRoom()
    {
        dialogueManager.EndDialogue();

        // freeze player controller

        if (scene.name == "Hospital" || scene.name == "VitalitySpace")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            playerCam.GetComponent<PlayerRotateToTarget>().enabled = false;
        }

        isCurrentlyExamine = false;

        if (scene.name == "ICU_New" || scene.name == "Hospital_Patient_SingleRoom" || scene.name == "2")
            currentRoom.GetComponentInChildren<ParticleSystem>().gameObject.SetActive(false);

        UI_ChoiceDia.SetActive(false);
        ScreenUnblur();

    }

    public void RoomComplete()
    {
        Stars[roomsWon++].StartAnimation();
        if (roomsWon >= 3)
            hospitalwin = true;

        currentRoom.door.doorlocked = false;
        currentRoom.completed = true;
        playerCam.GetComponent<PlayerRotateToTarget>().enabled = false;

        //if (scene.name == "Hospital_Patient_SingleRoom")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            playerCam.GetComponent<PlayerRotateToTarget>().enabled = true;
            globalStats.GOOD_stars += 1;
            currentRoom.gameObject.SetActive(false);
            globalStats.isMrJohnsonCompleted = true;
        }
    }

    public void RoomLost()
    {

        BadStars[roomsLost++].StartAnimation();
        if (roomsLost >= 3)
            Lose();

        currentRoom.door.doorlocked = false;
        currentRoom.lost = true;
        playerCam.GetComponent<PlayerRotateToTarget>().enabled = false;

        //if (scene.name == "Hospital_Patient_SingleRoom")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            playerCam.GetComponent<PlayerRotateToTarget>().enabled = true;
            globalStats.BAD_stars += 1;
            currentRoom.gameObject.SetActive(false);
            globalStats.isMrJohnsonCompleted = true;
        }
    }

    // on loss here
    // l   | l i
    // l l | l -
    public void Lose()
    {
        StartCoroutine(LoseWithDelay());
    }

    public void ReturnToMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Transition()
    {
        StartCoroutine(TransitionToCentral());
    }

    public IEnumerator LoseWithDelay()
    {

        yield return new WaitForSeconds(.05f);
        Canvas_Loss.SetActive(true);
        Debug.Log("loss");
        ScreenBlur();
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

    }

    public IEnumerator TransitionToCentral()
    {

        screenfade.SetBool("fade", true);

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Central");
    }

    public IEnumerator UpdateStars()
    {

        yield return new WaitForSeconds(0.2f);

        if (roomsWon >= 1)
        {
            Stars[0].JustDisplay();
            Debug.Log("star working");
        }

        if (roomsLost >= 1)
        {
            BadStars[0].JustDisplay();
            Debug.Log("bad star working");
        }

        if (roomsWon >= 2)
        {
            Stars[1].JustDisplay();
            Debug.Log("star working");
        }

        if (roomsLost >= 2)
        {
            BadStars[1].JustDisplay();
            Debug.Log("bad star working");
        }
        if (roomsWon >= 3)
        {
            Stars[2].JustDisplay();
            Debug.Log("star working");
        }

        if (roomsLost >= 3)
        {
            BadStars[2].JustDisplay();
            Debug.Log("bad star working");
        }
    }
}
