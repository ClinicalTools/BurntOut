using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu_GameManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

    public void StartGame() {

        Application.LoadLevel("Hospital");

    }
}
