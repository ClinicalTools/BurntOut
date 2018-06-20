using UnityEngine;

public class ExamineObject_Screen : MonoBehaviour
{
    private Camera mainCamera;

    private ParticleSystem myParticleSystem;

    private Transform startingTransform;
    private Main_GameManager gamemanager;

    public GameObject myCanvas;
    public GameObject myCanvasObject;

    public bool abilityToRunScript;


    // Use this for initialization
    void Start()
    {
        mainCamera = Camera.main;

        gamemanager = FindObjectOfType<Main_GameManager>();

        myParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        myParticleSystem.gameObject.SetActive(false);

        startingTransform = transform;
        myCanvasObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        abilityToRunScript = !gamemanager.isCurrentlyExamine;
    }

    private void OnMouseOver()
    {
        if (abilityToRunScript)
            myParticleSystem.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (abilityToRunScript)
            myParticleSystem.gameObject.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (abilityToRunScript)
        {
            gamemanager.ScreenBlur();
            myCanvas.SetActive(true);
            myCanvasObject.SetActive(true);

            gamemanager.isCurrentlyExamine = true;
        }
    }

    public void ExitExamine()
    {
        gamemanager.ScreenUnblur();
        myCanvas.SetActive(false);
        myCanvasObject.SetActive(false);

        gamemanager.isCurrentlyExamine = false;
        myParticleSystem.gameObject.SetActive(false);
    }
}