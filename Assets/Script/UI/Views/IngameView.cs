using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IngameView : BaseView
{
    public TextMeshProUGUI scoreTxt;
    public TextMeshProUGUI turnTxt;


    public override void Show()
    {
        base.Show();

        GameBroker.Ins.onUpdateScore += OnUpdateScore;
        GameBroker.Ins.onUpdateTurn += OnUpdateTurn;
    }

    public override void Hide()
    {
        GameBroker.Ins.onUpdateScore -= OnUpdateScore;
        GameBroker.Ins.onUpdateTurn -= OnUpdateTurn;

        base.Hide();
    }

    private void OnUpdateScore(int score)
    {
        scoreTxt.text = score.ToString();
    }

    private void OnUpdateTurn(int turn)
    {
        turnTxt.text = turn.ToString();
    }
}
