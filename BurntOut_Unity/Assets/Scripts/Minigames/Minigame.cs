﻿using UnityEngine;

namespace Minigames
{
    public abstract class Minigame : MonoBehaviour
    {
        public Main_GameManager gameManager;
        public bool completed;
        public GameObject gameUI;
        public PlayerStats playerStats;
        [HideInInspector]
        public string actionPrompt;

        public int plays;
        [HideInInspector]
        public int maxPlays;
        [HideInInspector]
        public int healthGain;

        public virtual void ResetGame()
        {
            plays = 0;
            completed = false;
        }
        public virtual void StartGame()
        {
            if (completed)
                return;

            gameUI.SetActive(true);
            gameManager.MinigameStart();
        }
        public virtual void ExitGame()
        {
            if (++plays >= maxPlays)
                completed = true;

            playerStats.CurrentHealth += healthGain;
            gameUI.SetActive(false);
            gameManager.MinigameEnd();
        }
    }
}