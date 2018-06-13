﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookNode : MonoBehaviour {

    public int id;
    //public bool start;
    public LookNode next;
    public LookNode behind;
    public LookNode currentlook = null;

    private WorldSpaceUI ws_ui;

    private Camera playerCamera;
    private PlayerRotateToTarget myRotateTo;
    private PlayerMoveToTarget myMoveTo;

	// Use this for initialization
	void Start () {

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myRotateTo = playerCamera.GetComponent<PlayerRotateToTarget>();
        myMoveTo = playerCamera.GetComponent<PlayerMoveToTarget>();

	}

    public void MoveTo() {
        myMoveTo.target = this.gameObject;
    }

    public void RotateNext() {
        myRotateTo.target = next.gameObject;
    }

    public void RotateBehind() {
        myRotateTo.target = behind.gameObject;
    }

}