using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DragExamine : MonoBehaviour {

    public float turnSpeed;
    private Vector3 startPos;

    void OnMouseDrag() {


        float rotationX = Input.GetAxis("Mouse X");
        float rotationY = Input.GetAxis("Mouse Y");

        //left and right
        if (Mathf.Abs(rotationX) > Mathf.Abs(rotationY)) {
            if (rotationX > 0)
                transform.Rotate(Vector3.up, -turnSpeed * Time.deltaTime);
            else
                transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
        }

        //up and down
        if (Mathf.Abs(rotationX) < Mathf.Abs(rotationY)) {
            if (rotationY < 0) {
                transform.Rotate(Vector3.right, -turnSpeed * Time.deltaTime);
            } else {
                transform.Rotate(Vector3.right, turnSpeed * Time.deltaTime);
            }

        }
    }
}
