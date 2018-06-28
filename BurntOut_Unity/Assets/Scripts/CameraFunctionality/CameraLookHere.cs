﻿using UnityEngine;

public class CameraLookHere : MonoBehaviour
{
    private Main_GameManager gamemanager;

    public float bounds = 1;
    private float xMax, yMax, xMin, yMin;
    private PlayerRotateToTarget myRotateTo;

    // Use this for initialization
    void Start()
    {
        var playerCamera = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        myRotateTo = playerCamera.GetComponent<PlayerRotateToTarget>();
        gamemanager = FindObjectOfType<Main_GameManager>();

        myRotateTo.enabled = true;
        myRotateTo.target = gameObject;

        xMax = transform.position.x + bounds;
        yMax = transform.position.y + bounds;
        xMin = transform.position.x - bounds;
        yMin = transform.position.y - bounds;
    }

    private void Update()
    {
        if (!gamemanager.isCurrentlyExamine)
        {

            Move();

        }
    }

    public int speed = 10;

    public void Move()
    {

        if (myRotateTo.target != null && myRotateTo.target != gameObject)
            return;

        myRotateTo.target = gameObject;
        
        var movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"), 0);

        transform.position += movement * speed * Time.deltaTime;

        var xPos = Mathf.Clamp(transform.position.x, xMin, xMax);
        var yPos = Mathf.Clamp(transform.position.y, yMin, yMax);
        transform.position = new Vector3(xPos, yPos, transform.position.z);
    }
}
