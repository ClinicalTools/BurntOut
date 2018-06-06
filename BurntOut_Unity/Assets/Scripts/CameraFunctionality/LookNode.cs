using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookNode : MonoBehaviour {

    public int id;
    public LookNode next;
    public LookNode behind;

    private WorldSpaceUI ws_ui;

    private Camera playerCamera;
    private PlayerRotateToTarget myRotate;

	// Use this for initialization
	void Start () {

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myRotate = playerCamera.GetComponent<PlayerRotateToTarget>();
        ws_ui = GameObject.FindGameObjectWithTag("WSUI").GetComponent<WorldSpaceUI>();

        if (id == 0) {
            myRotate.enabled = true;
            myRotate.target = this.gameObject;
            ws_ui.current = this;
        }

	}

    public void RotateNext() {
        myRotate.target = next.gameObject;
    }

    public void RotateBehind() {
        myRotate.target = behind.gameObject;
    }

}
