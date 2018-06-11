using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExamineObject_Screen : MonoBehaviour {

    private Camera mainCamera;

    private ParticleSystem myParticleSystem;

    private Transform startingTransform;
    private Main_GameManager gamemanager;

    public GameObject myCanvas;
    private bool abilityToRun;


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

        if (gamemanager.isCurrentlyExamine == false) {
            abilityToRun = true;
        }

        if (gamemanager.isCurrentlyExamine == true) {
            Debug.Log("akjsdhkasjdhaskajs");
            abilityToRun = false;
        }



    }

    private void OnMouseOver() {

        if (abilityToRun)
        myParticleSystem.gameObject.SetActive(true);

    }

    private void OnMouseExit() {

        if (abilityToRun)
        myParticleSystem.gameObject.SetActive(false);

    }

    private void OnMouseDown() {

        if (abilityToRun) {
            gamemanager.ScreenBlur();
            myCanvas.SetActive(true);

            gamemanager.isCurrentlyExamine = true;
        }


    }

    public void ExitExamine() {

        gamemanager.ScreenUnblur();
        myCanvas.SetActive(false);

        gamemanager.isCurrentlyExamine = false;
        myParticleSystem.gameObject.SetActive(false);
    }



}
