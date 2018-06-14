using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Star : MonoBehaviour {

    public GlobalStats globalStats;
    public bool GOODstar, BADstar;
 

    void Start () {

        globalStats = FindObjectOfType<GlobalStats>();

        //if (globalStats.GOOD_stars == 0 && globalStats.BAD_stars == 0)
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartAnimation() {
        gameObject.SetActive(true);
    }

    public void JustDisplay() {
        gameObject.GetComponent<Animator>().enabled = false;
        gameObject.SetActive(true);
    }
}
