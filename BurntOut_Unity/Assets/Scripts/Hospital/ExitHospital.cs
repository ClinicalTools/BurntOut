using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExitHospital : MonoBehaviour {


    public Main_GameManager gm;
    public GameObject Canvas_Win;
    public GameObject player;

    public bool isAroundStation;
    public bool playerFacing;
    public GameObject lookHere;

    public Text interactPrompt;

    public float maxAngle = 35;

    public GlobalStats globalstats;
    public Animator screenfade;

    private void Start() {
        globalstats = FindObjectOfType<GlobalStats>();
    }

    private void Update() {

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
        if (Input.GetKeyDown(KeyCode.E) && isAroundStation && playerFacing && gm.hospitalwin) {
            interactPrompt.transform.parent.gameObject.SetActive(false);

            // INTERACTION HERE
            Debug.Log("Game Completed");
            //Time.timeScale = 0;
            //Canvas_Win.SetActive(true);

            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;

            player.GetComponentInChildren<PlayerRotateToTarget>().target = lookHere;
            player.GetComponentInChildren<PlayerRotateToTarget>().enabled = true;

            globalstats.isHospitalCompleted = true;
            StartCoroutine(Transition());

        }
    }

    private void Look() {
        interactPrompt.transform.parent.gameObject.SetActive(true);
        interactPrompt.text = "Press 'e' to leave hospital";

        if (!gm.hospitalwin)
            interactPrompt.text = "You cannot leave yet!";
    }

    private void LookAway() {
        interactPrompt.transform.parent.gameObject.SetActive(false);
    }

    // detect if player is within interact range
    private void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Player")
            isAroundStation = true;
    }

    // return to default state when out of range
    private void OnTriggerExit(Collider col) {
        if (col.gameObject.name == "Player") {
            isAroundStation = false;
            playerFacing = false;
        }

        LookAway();
    }

    public IEnumerator Transition() {

        screenfade.SetBool("fade", true);

        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("Central");
    }
}
