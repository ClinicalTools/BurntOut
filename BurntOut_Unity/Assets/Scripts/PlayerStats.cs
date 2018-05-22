using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    // player stats here
    public float currentHealth;
    public float maxHealth = 100;


    private void Update() {
        
        if (currentHealth > 100) {
            currentHealth = 100;
        }

    }

}
