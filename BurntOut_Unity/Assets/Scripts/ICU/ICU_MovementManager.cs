using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICU_MovementManager : MonoBehaviour {

    public GameObject startingPosition;
    public GameObject startingRotation;
    public GameObject targetPosition;
    public GameObject targetRotation;

    public GameObject targetsNextMoveToButton;

    public PlayerMoveToTarget myMoveTo;
    public PlayerRotateToTarget myRotateTo;

    // 0 is starting location
    public int phase = 0;

    private void Start() {
        targetsNextMoveToButton.SetActive(false);
    }

    public void MoveAndRotate() {

        if (phase == 0) {
            MoveToTarget();
            RotateToTarget();
            phase = 1;
            targetsNextMoveToButton.SetActive(true);
        } else {
            MoveToTarget();
            RotateToTarget();
            phase = 0;
            targetsNextMoveToButton.SetActive(false);
        }
    }

    public void MoveToTarget() {

        if (phase == 0) {
            myMoveTo.enabled = true;
            myMoveTo.target = targetPosition;          
        }

        if (phase == 1) {
            myMoveTo.enabled = true;
            myMoveTo.target = startingPosition;
        }

    }

    public void RotateToTarget() {

        if (phase == 0) {
            myRotateTo.enabled = true;
            myRotateTo.target = targetRotation;
        }

        if (phase == 1) {
            myRotateTo.enabled = true;
            myRotateTo.target = startingRotation;
        }

    }
    

}
