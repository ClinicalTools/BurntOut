using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineObject : MonoBehaviour {

    private GameObject playersCloseExamineGameobject;
    private Camera mainCamera;

    private ParticleSystem myParticleSystem;

    private Transform startingTransform;


    // Use this for initialization
    void Start() {
        mainCamera = Camera.main;
        playersCloseExamineGameobject = GameObject.FindGameObjectWithTag("CloseExamine");

        myParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
        myParticleSystem.gameObject.SetActive(false);

        startingTransform = transform;
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnMouseOver() {

        myParticleSystem.gameObject.SetActive(true);


    }

    private void OnMouseExit() {

        myParticleSystem.gameObject.SetActive(false);

    }

    private void OnMouseDown() {


    }

}
