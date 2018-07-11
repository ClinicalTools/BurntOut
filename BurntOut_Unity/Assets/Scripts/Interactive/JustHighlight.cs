using UnityEngine;

public class JustHighlight : MonoBehaviour
{
    private ParticleSystem myParticleSystem;

    public bool ableToRunScript;

    public bool door;

    // hide this if door true
    public Material newDoorMaterial;
    public Material oldDoorMaterial;
    public GameObject myDoorMesh;

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
        if (!ableToRunScript) {
            myParticleSystem.gameObject.SetActive(false);
        }
    }

    private void OnMouseOver()
    {
        if (ableToRunScript) {
            myParticleSystem.gameObject.SetActive(true);

            if (door == true) {
                myDoorMesh.GetComponent<MeshRenderer>().material = newDoorMaterial;
            }
        }
    }

    private void OnMouseExit()
    {
        if (ableToRunScript) {
            myParticleSystem.gameObject.SetActive(false);

            if (door == true) {
                myDoorMesh.GetComponent<MeshRenderer>().material = oldDoorMaterial;
            }
        }
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
