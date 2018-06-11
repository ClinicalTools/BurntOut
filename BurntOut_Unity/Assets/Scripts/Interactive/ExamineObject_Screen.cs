using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineObject_Screen : MonoBehaviour {

    private Camera mainCamera;

    private ParticleSystem myParticleSystem;

    private Transform startingTransform;
    private Main_GameManager gamemanager;

    public GameObject myCanvas;


    // Use this for initialization
    void Start() {
        mainCamera = Camera.main;
        gamemanager = GameObject.FindObjectOfType<Main_GameManager>();

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

        gamemanager.ScreenBlur();
        myCanvas.SetActive(true);
    }

    public void ExitExamine() {

        gamemanager.ScreenUnblur();
        myCanvas.SetActive(false);

    }



}
