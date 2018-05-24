using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Reading
{
    public class ReadingGame : Minigame
    {
        public List<Book> books;
        public GameObject gameObj;
        public Text pageText;

        private int bookIndex;
        private int pageIndex;

        public int plays;

        public override void ResetGame()
        {
            bookIndex = 0;
            completed = false;
            plays = 0;
        }

        public override void StartGame()
        {
            if (completed || bookIndex > books.Count)
                return;

            gameObj.SetActive(true);
        }
    }
}