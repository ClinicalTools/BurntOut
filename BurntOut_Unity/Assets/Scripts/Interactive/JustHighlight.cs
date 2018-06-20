using UnityEngine;

public class JustHighlight : MonoBehaviour
{
    private ParticleSystem myParticleSystem;

    private Main_GameManager gamemanager;

    public bool ableToRunScript;
    // Use this for initialization
    void Start()
    {
        gamemanager = FindObjectOfType<Main_GameManager>();

        myParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        myParticleSystem.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ableToRunScript = !gamemanager.isCurrentlyExamine;
    }

    private void OnMouseOver()
    {
        if (ableToRunScript)
            myParticleSystem.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (ableToRunScript) { 
            myParticleSystem.gameObject.SetActive(false);
        }
    }

    private void OnMouseUpAsButton()
    {
        if (ableToRunScript)
        {
            gamemanager.ScreenBlur();

            gamemanager.isCurrentlyExamine = true;
        }
    }

    public void ExitExamine()
    {
        gamemanager.ScreenUnblur();

        gamemanager.isCurrentlyExamine = false;
        myParticleSystem.gameObject.SetActive(false);
    }

}
