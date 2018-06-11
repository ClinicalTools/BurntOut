using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateMe : MonoBehaviour {

    public Vector3 targetAngle = new Vector3(0f, 0f, 0f);
    public float speed;

    private Vector3 currentAngle;

    public void Start() {
        currentAngle = transform.eulerAngles;
    }

    public void Update() {

        currentAngle = new Vector3(

            Mathf.LerpAngle(currentAngle.x, targetAngle.x, speed * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.y, targetAngle.y, speed * Time.deltaTime),
            Mathf.LerpAngle(currentAngle.z, targetAngle.z, speed * Time.deltaTime));

        transform.eulerAngles = currentAngle;
    }
}