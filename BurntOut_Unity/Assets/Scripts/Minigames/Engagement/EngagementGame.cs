using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Engagement {

    public class EngagementGame : Minigame {

        public Button exitBtn;

        // Use this for initialization
        void Start() {
            maxPlays = 4;
            actionPrompt = "view screen";
            exitBtn.onClick.AddListener(delegate { ExitGame(); });

        }

        public override void StartGame() {
            gameManager.MinigameStart();
            gameManager.ScreenUnblur();
            gameUI.SetActive(true);
        }

        // Update is called once per frame
        void Update() {

        }

    }

}
