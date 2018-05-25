using UnityEngine;
using UnityEngine.UI;

namespace Minigames
{
    public abstract class Minigame : MonoBehaviour
    {
        public Main_GameManager gameManager;
        public bool completed;
        public GameObject gameUI;
        public PlayerStats playerStats;
        public Text interactPrompt;
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
            
            interactPrompt.transform.parent.gameObject.SetActive(false);

            gameUI.SetActive(true);
            gameManager.MinigameStart();
        }
        public virtual void ExitGame()
        {
            if (++plays >= maxPlays)
                completed = true;
            else
                interactPrompt.transform.parent.gameObject.SetActive(true);

            playerStats.CurrentHealth += healthGain;
            gameUI.SetActive(false);
            gameManager.MinigameEnd();
        }
    }
}