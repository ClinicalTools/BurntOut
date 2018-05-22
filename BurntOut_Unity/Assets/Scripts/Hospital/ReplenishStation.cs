using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplenishStation : MonoBehaviour {

    public int patientNumber;
    public bool completed;
    public bool isAround;

    public GameObject player;
    private PlayerStats playerStats;

    private void Start() {

        playerStats = player.GetComponent<PlayerStats>();

    }

    void Update() {

        // if player is around patient, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E)) {

            if (isAround == true) {

                // INTERACTION HERE
                playerStats.currentHealth += 10;




            }

        }

    }


    // detect if player is within interact range
    void OnTriggerEnter(Collider col) {

        if (col.gameObject.name == "Player") {

            Debug.Log("Player Detected");
            isAround = true;

        } else {

            Debug.Log("Other obj detected");
            isAround = false;

        }

    }

    // return to default state when out of range
    void OnTriggerExit(Collider col) {

        Debug.Log("Player away from patient");
        isAround = false;

    }
}
