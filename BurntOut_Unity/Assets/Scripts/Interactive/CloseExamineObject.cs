using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloseExamineObject : MonoBehaviour {

    private GameObject playersCloseExamineGameobject;
    private Camera mainCamera;
    private PlayerMoveToTarget myMoveToTarget;
    private PlayerRotateToTarget myRotateToTarget;

    private Transform startingTransform;


	// Use this for initialization
	void Start () {
        mainCamera = Camera.main;
        playersCloseExamineGameobject = GameObject.FindGameObjectWithTag("CloseExamine");
        myMoveToTarget = gameObject.GetComponent<PlayerMoveToTarget>();
        myRotateToTarget = gameObject.GetComponent<PlayerRotateToTarget>();

        startingTransform = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnMouseOver() {

        Debug.Log("Hovering");

    }

    private void OnMouseDown() {

        myMoveToTarget.enabled = true;
        myRotateToTarget.enabled = true;

        myMoveToTarget.target = playersCloseExamineGameobject;
        myRotateToTarget.target = mainCamera.gameObject;

    }

    public void ReturnToOriginal() {

        transform.position = Vector3.Lerp(transform.position, startingTransform.position, Time.deltaTime * 2); // lerp from A to B in one second

    }

}
