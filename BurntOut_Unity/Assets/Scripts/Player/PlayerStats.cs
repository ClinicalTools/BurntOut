using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    // player stats here
    [SerializeField]
    private int currentHealth = 100;
    public int maxHealth = 100;

    public int CurrentHealth
    {
        get
        {
            return currentHealth;
        }

        set
        {
            currentHealth = value;

            if (currentHealth > 100)
                currentHealth = 100;
            if (currentHealth < 0)
                currentHealth = 0;
        }
    }

    private void Start()
    {
        currentHealth = maxHealth;
    }
}
