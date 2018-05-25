using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Minigames.Reading
{
    public class ReadingGame : Minigame
    {
        public List<Book> books;
        public Image pageImage;
        public Text pageText;
        public Button lastPageBtn;
        public Button nextPageBtn;
        public Button exitBtn;


        private int bookIndex;
        private int pageIndex;

        private void Start()
        {
            maxPlays = 4;
            healthGain = 10;
            actionPrompt = "read a book";

            lastPageBtn.onClick.RemoveAllListeners();
            lastPageBtn.onClick.AddListener(delegate { LastPage(); });
            nextPageBtn.onClick.RemoveAllListeners();
            nextPageBtn.onClick.AddListener(delegate { NextPage(); });
            exitBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.AddListener(delegate { ExitGame(); });
        }
        
        public override void StartGame()
        {
            if (bookIndex >= books.Count)
            {
                return;
            }

            pageIndex = 0;
            lastPageBtn.gameObject.SetActive(false);
            nextPageBtn.gameObject.SetActive(true);
            exitBtn.gameObject.SetActive(false);

            UpdatePage();

            base.StartGame();
        }

        public override void ExitGame()
        {
            bookIndex++;

            base.ExitGame();
        }

        public void LastPage()
        {
            pageIndex--;

            if (pageIndex == 0)
                lastPageBtn.gameObject.SetActive(false);

            nextPageBtn.gameObject.SetActive(true);

            UpdatePage();
        }

        public void NextPage()
        {
            pageIndex++;

            if (pageIndex == books[bookIndex].pages.Length - 1)
            {
                exitBtn.gameObject.SetActive(true);
                nextPageBtn.gameObject.SetActive(false);
            }

            lastPageBtn.gameObject.SetActive(true);

            UpdatePage();
        }

        public void UpdatePage()
        {
            pageText.text = books[bookIndex].pages[pageIndex].text;
            pageImage.sprite = books[bookIndex].pages[pageIndex].img;
        }

    }
}