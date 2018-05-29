using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Star : MonoBehaviour {



	// Use this for initialization
	void Start () {
        gameObject.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartAnimation() {
        gameObject.SetActive(true);
    }
}
