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

            base.StartGame();

            gameManager.ScreenUnblur();
            
        }

        // Update is called once per frame
        void Update() {

        }

    }

}
