using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHospital : MonoBehaviour {


    public Main_GameManager gm;
    public GameObject Canvas_Win;
    public GameObject player;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        
	}

    private void OnTriggerEnter(Collider other) {
        
        if (other.tag == "Player" && gm.hospitalwin == true) {
            Debug.Log("Game Completed");
            //Time.timeScale = 0;
            Canvas_Win.SetActive(true);
            player.GetComponent<UnityStandardAssets.Characters.FirstPerson.FirstPersonController>().enabled = false;
        }

    }
}
