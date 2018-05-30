using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthAlert : MonoBehaviour {

    public PlayerStats playerstats;
    public GameObject mytext;

	// Update is called once per frame
	void Update () {

        if (playerstats.CurrentHealth < 40) {
            mytext.SetActive(true);
        } else
            mytext.SetActive(false);


    }
}
