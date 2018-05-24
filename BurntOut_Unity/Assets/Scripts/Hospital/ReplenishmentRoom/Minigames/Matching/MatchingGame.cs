using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Matching
{
    public class MatchingGame : Minigame
    {
        public Button exitBtn;

        public void Start()
        {
            maxPlays = 2;
            actionPrompt = "match things";
            
            exitBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.AddListener(delegate { ExitGame(); });
        }
    }
}
