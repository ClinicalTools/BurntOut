using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineObject_Canvas : MonoBehaviour {


    public float distanceFromCamera = 2;
    private GameObject targetlocation;
    private Camera mainCamera;


    public void Start() {
        mainCamera = Camera.main;
        targetlocation = GameObject.FindGameObjectWithTag("CloseExamine");

        StartCoroutine(SetPosAndRot());
    }

    public IEnumerator SetPosAndRot() {

        transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
        gameObject.GetComponent<PlayerRotateToTarget>().enabled = true;

        yield return new WaitForSeconds(0.2f);

        gameObject.GetComponent<PlayerRotateToTarget>().enabled = false;

    }

    private void Update() {
        transform.position = mainCamera.transform.position + mainCamera.transform.forward * distanceFromCamera;
    }
}
