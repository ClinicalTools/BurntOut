using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLookHere : MonoBehaviour {

    private Camera playerCamera;
    private PlayerRotateToTarget myRotateTo;
    private PlayerMoveToTarget myMoveTo;
    private Main_GameManager gamemanager;

    public float bounds = 1;
    private float Xmax;
    private float Ymax;
    private float Xmin;
    private float Ymin;

    // Use this for initialization
    void Start() {

        playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myRotateTo = playerCamera.GetComponent<PlayerRotateToTarget>();
        myMoveTo = playerCamera.GetComponent<PlayerMoveToTarget>();
        gamemanager = FindObjectOfType<Main_GameManager>();

        myRotateTo.enabled = true;
        myRotateTo.target = this.gameObject;

        Xmax = transform.position.x + bounds;
        Ymax = transform.position.y + bounds;
        Xmin = transform.position.x - bounds;
        Ymin = transform.position.y - bounds;

    }

    private void Update() {

        if (!gamemanager.isCurrentlyExamine) {

            Move();

            if (transform.position.x >= Xmax) {
                transform.position = new Vector3(Xmax, transform.position.y, transform.position.z);
            }

            if (transform.position.y >= Ymax) {
                transform.position = new Vector3(transform.position.x, Ymax, transform.position.z);
            }

            if (transform.position.x <= Xmin) {
                transform.position = new Vector3(Xmin, transform.position.y, transform.position.z);
            }

            if (transform.position.y <= Ymin) {
                transform.position = new Vector3(transform.position.x, Ymin, transform.position.z);
            }
        }
    }

    public int speed = 10;

    public void Move() {

        Vector3 Movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        transform.position += Movement * speed * Time.deltaTime;

    }


}
