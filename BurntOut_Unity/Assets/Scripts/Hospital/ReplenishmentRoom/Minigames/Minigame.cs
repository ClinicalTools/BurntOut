using UnityEngine;

namespace Minigames
{
    public abstract class Minigame : MonoBehaviour
    {
        public bool completed;
        public string actionPrompt;

        public abstract void ResetGame();
        public abstract void StartGame();
    }
}