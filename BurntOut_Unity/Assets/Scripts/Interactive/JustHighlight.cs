using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JustHighlight : MonoBehaviour {

    private Camera mainCamera;

    private ParticleSystem myParticleSystem;

    private Transform startingTransform;
    private Main_GameManager gamemanager;

    public bool abilityToRunScript;


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
            abilityToRunScript = true;
        }

        if (gamemanager.isCurrentlyExamine == true) {
            abilityToRunScript = false;
        }



    }

    private void OnMouseOver() {

        if (abilityToRunScript)
            myParticleSystem.gameObject.SetActive(true);

    }

    private void OnMouseExit() {

        if (abilityToRunScript)
            myParticleSystem.gameObject.SetActive(false);

    }

    private void OnMouseDown() {

        if (abilityToRunScript) {
            gamemanager.ScreenBlur();


            gamemanager.isCurrentlyExamine = true;
        }


    }

    public void ExitExamine() {

        gamemanager.ScreenUnblur();


        gamemanager.isCurrentlyExamine = false;
        myParticleSystem.gameObject.SetActive(false);
    }

}
