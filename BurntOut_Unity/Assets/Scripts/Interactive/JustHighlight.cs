using UnityEngine;

public class JustHighlight : MonoBehaviour
{
    private ParticleSystem myParticleSystem;

    public bool ableToRunScript;
    // Use this for initialization
    void Start()
    {
        myParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        myParticleSystem.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        ableToRunScript = !Main_GameManager.Instance.isCurrentlyExamine;
        if (!ableToRunScript)
            myParticleSystem.gameObject.SetActive(false);

    }

    private void OnMouseOver()
    {
        if (ableToRunScript)
            myParticleSystem.gameObject.SetActive(true);
    }

    private void OnMouseExit()
    {
        if (ableToRunScript) 
            myParticleSystem.gameObject.SetActive(false);
    }

    private void OnMouseUpAsButton()
    {
        if (ableToRunScript)
        {
            myParticleSystem.gameObject.SetActive(false);
        }
    }

    public void ExitExamine()
    {
        myParticleSystem.gameObject.SetActive(false);
    }

}
