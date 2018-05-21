using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour {

    public PlayerStats playerStats;

    // text display
    private float currentHealth;
    private float maxHealth;
    public Text displayText;

    // healthbar fill
    public Image image;
    public float fillAmount;

	void Update () {

        // update text of health 
        currentHealth = playerStats.currentHealth;
        maxHealth = playerStats.maxHealth;

        displayText.text = currentHealth.ToString() + " / " + maxHealth.ToString();

	}

    void FixedUpdate() {

        // update "fill" or image of health for increased visual feedback
        fillAmount = (playerStats.currentHealth / playerStats.maxHealth);
        image.fillAmount = fillAmount;
        
    }
}
