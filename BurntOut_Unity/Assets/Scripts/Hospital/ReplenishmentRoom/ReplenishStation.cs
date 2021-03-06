﻿using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityStandardAssets.Characters.FirstPerson;

public class ReplenishStation : MonoBehaviour
{
    public bool isAroundStation;
    public bool playerFacing;

    public Text interactPrompt;
    public FirstPersonController fpc;
    
    public Transform replenishRoomSpawn;
    public ReplenishTeleport replenishTeleport;

    public float maxAngle = 35;

    public Animator myAnimator;
    public bool trigger = true;

    void Update()
    {
        if (isAroundStation)
        {
            Vector3 vec = transform.position - fpc.transform.position;
            vec.y = 0;

            // Measure from the direction opposite the player
            Vector3 playerDir = fpc.transform.TransformDirection(Vector3.forward);
            playerDir.y = 0;

            bool wasFacing = playerFacing;
            playerFacing = Vector3.Angle(playerDir, vec) < maxAngle;

            if (wasFacing && !playerFacing)
                LookAway();
            else if (!wasFacing && playerFacing)
                Look();
        }

        // if player is around patient, allow player to interact with it
        if (Input.GetKeyDown(KeyCode.E) && isAroundStation && playerFacing)
        {
            interactPrompt.transform.parent.gameObject.SetActive(false);

            // INTERACTION HERE
            StartCoroutine(Transition());


            LookAway();
        }
    }

    private void Look()
    {
        interactPrompt.transform.parent.gameObject.SetActive(true);
        interactPrompt.text = "Press 'e' to go to the replenishment room";
    }

    private void LookAway()
    {
        interactPrompt.transform.parent.gameObject.SetActive(false);
    }

    // detect if player is within interact range
    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Player")
            isAroundStation = true;
    }

    // return to default state when out of range
    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.name == "Player")
        {
            isAroundStation = false;
            playerFacing = false;

            LookAway();
        }
    }

    public IEnumerator Transition()
    {
        fpc.enabled = false;
        myAnimator.SetBool("fade", true);
        yield return new WaitForSeconds(0.5f);
        replenishTeleport.gameObject.SetActive(true);
        replenishTeleport.teleportPos = transform.position;
        replenishTeleport.teleportRot = transform.rotation;
        fpc.transform.SetPositionAndRotation(replenishRoomSpawn.transform.position, replenishRoomSpawn.transform.rotation);
        fpc.ResetRotation();
        yield return new WaitForSeconds(0.5f);
        myAnimator.SetBool("fade", false);
        fpc.enabled = true;
    }
}
