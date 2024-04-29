using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public int row, col;

    [SerializeField] private CardManager cardManager;

    public AudioSource matchSfx;
    public AudioSource misMatchSfx;
    public AudioSource gameOverSfx;

    private Card flippedCard;
    private bool isStarted = false;

    private int turn = 0;
    private int score = 0;

    private void Start()
    {
        ViewManager.Ins.Show("Menu");
    }

    private void OnApplicationQuit()
    {
        if (isStarted)
            SaveSystem.SaveCardsData(cardManager.GetCardDatasIngame(), turn, score);
    }

    public void Prepare()
    {
        var saveData = SaveSystem.LoadCardsData();

        if (saveData != null && saveData.data.Count > 0)
        {
            turn = saveData.turn;
            score = saveData.score;
            cardManager.SetupCards(row, col, saveData.data);
        }
        else
        {
            turn = 0;
            score = 0;
            cardManager.SetupCards(row, col);
        }

        cardManager.onFlip = OnFlip;
        cardManager.FlipUpAllCards();

        ViewManager.Ins.Show("Ingame");

        GameBroker.Ins.onUpdateScore.Invoke(score);
        GameBroker.Ins.onUpdateTurn.Invoke(turn);

        Startgame();
    }

    private void Startgame()
    {
        StartCoroutine(DelayStart());
    }

    public void Endgame()
    {
        gameOverSfx.Play();
        SaveSystem.SaveCardsData(new(), 0, 0);
        isStarted = false;
        ViewManager.Ins.Show("Menu");
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1);
        cardManager.FlipBackAllCards();
        isStarted = true;
    }
    private void Update()
    {
        if (isStarted) return;

        if (Input.GetKeyDown(KeyCode.R))
        {
            Prepare();
        }
    }

    private void OnFlip(Card card)
    {
        if (flippedCard != null)
        {
            if (flippedCard == card)
            {
                flippedCard = null;
            }
            else
            {
                if (card.CardId == flippedCard.CardId)
                {
                    matchSfx.Play();
                    MatchCard(flippedCard, card);
                }
                else
                {
                    StartCoroutine(FlipBackTwoCards(flippedCard, card));
                }

                flippedCard = null;
                turn++;
                GameBroker.Ins.onUpdateTurn?.Invoke(turn);
            }
        }
        else
        {
            flippedCard = card;
        }
    }

    private void MatchCard(Card flippedCard, Card card)
    {
        flippedCard.MatchCard();
        card.MatchCard();
        score++;
        GameBroker.Ins.onUpdateScore?.Invoke(score);
        CheckEndgame();
    }

    private void CheckEndgame()
    {
        if (cardManager.AllCardMatched()) Endgame();
    }

    IEnumerator FlipBackTwoCards(Card card1, Card card2)
    {
        yield return new WaitUntil(() => !card1.IsRotating && !card2.IsRotating);
        misMatchSfx.Play();
        card1.FlipBack();
        card2.FlipBack();
    }
}
