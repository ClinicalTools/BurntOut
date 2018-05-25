using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthBar : MonoBehaviour
{

    public PlayerStats playerStats;

    // text display
    private float displayHealth;
    private float oldHealth;
    private float currentHealth;
    private float maxHealth;
    public Text displayText;

    // healthbar fill
    public Image image;
    public float fillAmount;

    private void Start()
    {
        displayHealth = playerStats.CurrentHealth;
        oldHealth = playerStats.CurrentHealth;
        currentHealth = playerStats.CurrentHealth;
    }

    void Update()
    {
        displayText.text = (int)displayHealth + " / " + playerStats.maxHealth;
    }

    // Time it takes for health bar to finish moving
    float changeTime = 1f;
    float t;
    void FixedUpdate()
    {
        // Health done moving
        if (t > 1)
        {
            oldHealth = currentHealth;
            t = 0;
        }
        // Player's health changed
        if (currentHealth != playerStats.CurrentHealth)
        {
            t = 0;
            oldHealth = displayHealth;
            currentHealth = playerStats.CurrentHealth;
        }
        // Bar is moving
        if (oldHealth != currentHealth)
        {
            t += (Time.deltaTime) / changeTime;
            displayHealth = Mathf.Lerp(oldHealth, currentHealth, t);
        }



        // update "fill" or image of health for increased visual feedback
        image.fillAmount = displayHealth / playerStats.maxHealth;
        
    }
}
