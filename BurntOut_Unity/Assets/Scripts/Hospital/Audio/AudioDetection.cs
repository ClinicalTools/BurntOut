using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioDetection : MonoBehaviour {

    public Animator myAnimator;

    // detect if player is within interact range
    void OnTriggerEnter(Collider col) {
        if (col.gameObject.name == "Player") {
            myAnimator.SetBool("PlayerInZone", true);
        }


    }

    // return to default state when out of range
    void OnTriggerExit(Collider col) {
        if (col.gameObject.name == "Player") {
            myAnimator.SetBool("PlayerInZone", false);
        }
        
    }
}
