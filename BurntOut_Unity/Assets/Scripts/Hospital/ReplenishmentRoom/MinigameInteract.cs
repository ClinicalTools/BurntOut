using Minigames;
using UnityEngine;
using UnityEngine.UI;

public class MinigameInteract : MonoBehaviour
{

    public bool isAroundStation;
    public bool playerFacing;

    public Text interactPrompt;
    public GameObject player;
    public Minigame minigame;
    public Camera playerCamera;

    // for placing reading minigame infront of camera
    public float bookDistance = 1.0f;
    public float bookSpeed = 0.10f;


    public float maxAngle = 35;

    void Update()
    {

        if (isAroundStation)
        {
            Vector3 vec = transform.position - player.transform.position;
            vec.y = 0;

            // Measure from the direction opposite the player
            Vector3 playerDir = player.transform.TransformDirection(Vector3.forward);
            playerDir.y = 0;

            bool wasFacing = playerFacing;
            playerFacing = Vector3.Angle(playerDir, vec) < maxAngle;

            if (wasFacing && !playerFacing)
                LookAway();
            else if (!wasFacing && playerFacing && !minigame.completed)
                Look();
        }

        // if player is around patient, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E) && isAroundStation && playerFacing && !minigame.completed)
        {
            interactPrompt.transform.parent.gameObject.SetActive(false);

            // INTERACTION HERE
            minigame.enabled = true;

            minigame.StartGame();
            player.GetComponent<PlayerRotateToTarget>().target = gameObject;
            // make book move to screen and face player               
            // below works for just placing book infront of player
            //this.transform.position = playerCamera.transform.position + playerCamera.transform.forward * bookDistance;
            //this.transform.rotation = new Quaternion(0.0f, playerCamera.transform.rotation.y, 0.0f, playerCamera.transform.rotation.w);
        }
    }

    private void Look()
    {
        interactPrompt.transform.parent.gameObject.SetActive(true);
        interactPrompt.text = "Press 'e' to " + minigame.actionPrompt;
    }

    private void LookAway()
    {
        interactPrompt.transform.parent.gameObject.SetActive(false);
    }

    // detect if player is within interact range
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
            isAroundStation = true;
    }

    // return to default state when out of range
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            isAroundStation = false;
            playerFacing = false;
        }

        LookAway();
    }
}

