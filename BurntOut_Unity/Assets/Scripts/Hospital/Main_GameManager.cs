using UnityEngine;
using UnityEngine.PostProcessing;
using UnityEngine.SceneManagement;
using System.Collections;

public class Main_GameManager : MonoBehaviour
{
    public static Main_GameManager Instance { get; private set; }

    private int roomsWon;
    private int roomsLost;

    // store current patient

    public Animator screenfade;

    public GameObject Canvas_Paused;
    public GameObject Canvas_Win;
    public GameObject Canvas_Loss;

    // UIs
    public GameObject UI_ReadingStation;
    public GameObject UI_MatchingStation;

    // Feedback
    public TextTyper FeedbackTyper;

    // Win UI
    public UI_Star[] Stars;
    public UI_Star[] BadStars;

    //player controller
    public GameObject player;
    public GameObject playerCam;
    public PlayerStats playerStats;
    public PostProcessingBehaviour ppScene;

    DepthOfFieldModel.Settings dofSettings;

    public bool gameover;
    public bool gamePaused;

    public bool hospitalwin;

    public GlobalStats globalStats;

    // for examine objects
    public bool isCurrentlyExamine;

    public Scene scene;

    private void Awake()
    {
        Instance = this;

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

        if (scene.name == "Hospital" || scene.name == "Central")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        }

        if (scene.name == "VitalitySpace")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        }
        
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
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gamePaused == true)
            {
                Debug.Log("Resume Game");
                Time.timeScale = 1;
                gamePaused = false;
                Canvas_Paused.SetActive(false);

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

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

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;

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
        player.GetComponentInChildren<PlayerMovement>().enabled = true;
    }

    public void MinigameEnd()
    {
        ScreenUnblur();
        player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        player.GetComponentInChildren<PlayerMovement>().enabled = false;
    }

    ///////////////////////////////////////////////////////////////////////////
    //////////////////////////////// MINIGAMES ////////////////////////////////
    ///////////////////////////////////////////////////////////////////////////



    public void ScreenBlur()
    {
        dofSettings.focusDistance = 0.1f;
        dofSettings.aperture = 14;
        dofSettings.focalLength = 21;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

    public void ScreenUnblur()
    {
        dofSettings.focusDistance = 0.94f;
        dofSettings.aperture = 5;
        // Somehow this setting made things start to appear blurry, and I have no idea why
        //dofSettings.focalLength = 50;
        dofSettings.focalLength = 1;
        ppScene.profile.depthOfField.settings = dofSettings;
    }

    public void ExitRoom()
    {
        // freeze player controller

        if (scene.name == "Hospital" || scene.name == "VitalitySpace")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = true;
            playerCam.GetComponent<PlayerMovement>().enabled = false;
        }

        isCurrentlyExamine = false;

        ScreenUnblur();

    }

    public void RoomComplete()
    {
        Stars[roomsWon++].StartAnimation();
        if (roomsWon >= 3)
            hospitalwin = true;

        playerCam.GetComponent<PlayerMovement>().enabled = false;

        //if (scene.name == "Hospital_Patient_SingleRoom")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            playerCam.GetComponent<PlayerMovement>().enabled = true;
            globalStats.GOOD_stars += 1;
            globalStats.isMrJohnsonCompleted = true;
        }
    }

    public void ExitGame() {
        Application.Quit();
    }

    public void RoomLost()
    {

        BadStars[roomsLost++].StartAnimation();
        if (roomsLost >= 3)
            Lose();

        playerCam.GetComponent<PlayerMovement>().enabled = false;

        //if (scene.name == "Hospital_Patient_SingleRoom")
        {
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
            playerCam.GetComponent<PlayerMovement>().enabled = true;
            globalStats.BAD_stars += 1;
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

    private string nextScene;
    public void Transition(string nextScene = "Central")
    {
        this.nextScene = nextScene; 
        StartCoroutine(TransitionToScene());
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
    
    public IEnumerator TransitionToScene()
    {
        screenfade.SetBool("fade", true);

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene(nextScene);
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
