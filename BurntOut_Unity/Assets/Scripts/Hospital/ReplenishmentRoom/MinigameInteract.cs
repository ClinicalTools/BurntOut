using Minigames;
using UnityEngine;
using UnityEngine.UI;

public class MinigameInteract : MonoBehaviour {

    public bool isAroundStation;
    public bool playerFacing;

    public Text interactPrompt;
    public GameObject player;
    public Minigame minigame;

    public Main_GameManager gamemanager;

    public float maxAngle = 35;

    void Update() {

        if (isAroundStation) {
            Vector3 vec = transform.position - player.transform.position;
            vec.y = 0;

            // Measure from the direction opposite the player
            Vector3 playerDir = player.transform.TransformDirection(Vector3.forward);
            playerDir.y = 0;

            bool wasFacing = playerFacing;
            playerFacing = Vector3.Angle(playerDir, vec) < maxAngle;

            if (wasFacing && !playerFacing)
                LookAway();
            else if (!wasFacing && playerFacing)
                Look();
        }

        // if player is around patient, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E) && isAroundStation && playerFacing) {
            interactPrompt.transform.parent.gameObject.SetActive(false);

            // INTERACTION HERE
            minigame.enabled = true;

            gamemanager.ReadingStation_Start();         
            player.GetComponent<PlayerRotateToTarget>().target = gameObject;

            LookAway();
        }
    }

    private void Look() {
        interactPrompt.transform.parent.gameObject.SetActive(true);
        interactPrompt.text = "Press 'e' to " + minigame.actionPrompt;
    }

    private void LookAway() {
        interactPrompt.transform.parent.gameObject.SetActive(false);
    }

    // detect if player is within interact range
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Player")
            isAroundStation = true;
    }

    // return to default state when out of range
    void OnTriggerExit(Collider col) {
        if (col.gameObject.name == "Player") {
            isAroundStation = false;
            playerFacing = false;
        }

        LookAway();
    }
}

