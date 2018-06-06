﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldSpaceUI : MonoBehaviour {

    public GameObject player;
    public Camera playerCamera;

    public float rotationSpeed = 1f;
    public bool rotateCameraRight;

    public LookNode current;

    public void RotateNext() {
        current.RotateNext();
        current = current.next;
    }

    public void RotateBehind() {
        current.RotateBehind();
        current = current.behind;
    }

    public void MoveTo() {
        current.MoveTo();
        if (current.currentlook != null) {
            current = current.currentlook;
            playerCamera.GetComponent<PlayerRotateToTarget>().target = current.gameObject;
        }
    }



}
