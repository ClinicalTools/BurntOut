using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Central_WSUI : MonoBehaviour {

    public GameObject player;
    public Camera playerCamera;

    public float rotationSpeed = 1f;
    public bool rotateCameraRight;

    private void Update() {

        if (rotateCameraRight) {
            StartCoroutine(Rotate(Vector3.up, -90, 1));
        }


    }

    public void RotateCameraRight() {

        rotateCameraRight = true;

    }

    IEnumerator Rotate(Vector3 axis, float angle, float duration = 1.0f) {

        Quaternion from = playerCamera.transform.rotation;
        Quaternion to = new Quaternion(0, playerCamera.transform.rotation.y+90, 0, 0);
        to *= Quaternion.Euler(axis * angle);

        float elapsed = 0.0f;

        while (elapsed < duration) {
            playerCamera.transform.rotation = Quaternion.Lerp(from, to, elapsed / duration );
            elapsed += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.rotation = to;
        rotateCameraRight = false;

    }




}
