using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ICU_MovementManager : MonoBehaviour {

    public GameObject startingPosition;
    public GameObject playerLookHere1;
    public GameObject playerLookHere2;
    public GameObject startingRotation;
    public GameObject targetPosition;


    public GameObject targetsNextMoveToButton;

    public PlayerMoveToTarget myMoveTo;
    public PlayerRotateToTarget myRotateTo;

    // 0 is starting location
    public int phase = 0;

    private void Start() {
        targetsNextMoveToButton.SetActive(false);
        playerLookHere1.SetActive(true);
        playerLookHere2.SetActive(false);
    }

    public void MoveAndRotate() {

        if (phase == 0) {
            MoveToTarget();
            //RotateToTarget();
            phase = 1;
            targetsNextMoveToButton.SetActive(true);
        } else {
            MoveToTarget();
            //RotateToTarget();
            phase = 0;
            targetsNextMoveToButton.SetActive(false);
        }
    }

    public void MoveToTarget() {

        if (phase == 0) {
            myMoveTo.enabled = true;
            myMoveTo.target = targetPosition;

            playerLookHere1.SetActive(false);
            playerLookHere2.SetActive(true);
        }

        if (phase == 1) {
            myMoveTo.enabled = true;
            myMoveTo.target = startingPosition;

            playerLookHere1.SetActive(true);
            playerLookHere2.SetActive(false);
        }

    }

    /*

    public void RotateToTarget() {

        if (phase == 0) {
            myRotateTo.enabled = true;
            myRotateTo.target = playerLookHere1;
        }

        if (phase == 1) {
            myRotateTo.enabled = true;
            myRotateTo.target = playerLookHere2;
        }

    }

    */
    

}
