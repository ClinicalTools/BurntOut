using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using System.Security.Cryptography;
namespace Minigames.Matching
{
    public class MatchingGame : Minigame
    {
        private const int ANSWER_COUNT = 10;

        public string[] answerStrs = new string[ANSWER_COUNT];
        public Button[] cardButtons = new Button[ANSWER_COUNT * 2];
        private Text[] cardTexts = new Text[ANSWER_COUNT * 2];
        private bool[] cardMatched = new bool[ANSWER_COUNT * 2];
        private int[] cardVals = new int[ANSWER_COUNT * 2];
        public Button exitBtn;

        private void Start()
        {
            maxPlays = 2;
            healthGain = 20;
            actionPrompt = "match things";

            exitBtn.onClick.RemoveAllListeners();
            exitBtn.onClick.AddListener(delegate { ExitGame(); });

            for (int i = 0; i < cardButtons.Length; i++)
            {
                cardTexts[i] = cardButtons[i].GetComponentInChildren<Text>();
                cardButtons[i].onClick.RemoveAllListeners();
                var buttonNum = i;
                cardButtons[i].onClick.AddListener(delegate { ClickCard(buttonNum); });
            }
        }

        public override void StartGame()
        {
            exitBtn.gameObject.SetActive(false);

            cardMatched = new bool[ANSWER_COUNT * 2];
            cardVals = new int[ANSWER_COUNT * 2];
            for (int i = 0; i < cardVals.Length; i++)
                cardVals[i] = i % ANSWER_COUNT;
            cardVals.Shuffle();

            for (int i = 0; i < cardVals.Length; i++)
            {
                cardButtons[i].interactable = true;
                cardTexts[i].text = "";
            }

            base.StartGame();
        }

        private int lastCard = -1;
        private void ClickCard(int selectedCard)
        {
            cardButtons[selectedCard].interactable = false;
            cardTexts[selectedCard].text = answerStrs[cardVals[selectedCard]];

            if (lastCard < 0)
            {
                lastCard = selectedCard;
            }
            else if (cardVals[lastCard] == cardVals[selectedCard])
            {
                cardMatched[lastCard] = true;
                cardMatched[selectedCard] = true;
                lastCard = -1;

                bool victory = true;
                foreach (var matched in cardMatched)
                {
                    if (!matched)
                    {
                        victory = false;
                        break;
                    }
                }

                if (victory)
                    exitBtn.gameObject.SetActive(true);
            }
            else
            {
                { 
                    int cardA = lastCard;
                    int cardB = selectedCard;
                    StartCoroutine(FlipDown(cardA, cardB));
                }
                lastCard = -1;
            }
        }

        private IEnumerator FlipDown(int cardA, int cardB)
        {
            yield return new WaitForSeconds(1f);

            cardButtons[cardA].interactable = true;
            cardButtons[cardB].interactable = true;
            cardTexts[cardA].text = "";
            cardTexts[cardB].text = "";
        }

        private void OnValidate()
        {
            Array.Resize(ref cardButtons, ANSWER_COUNT * 2);
            Array.Resize(ref answerStrs, ANSWER_COUNT);
        }
    }

    static class MyExtensions
    {
        public static void Shuffle<T>(this T[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                int oldPos = Mathf.FloorToInt(UnityEngine.Random.Range(i, arr.Length));

                T val = arr[i];
                arr[i] = arr[oldPos];
                arr[oldPos] = val;
            }
        }
    }
}
