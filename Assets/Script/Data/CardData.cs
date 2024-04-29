using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CardData
{
    public int id;
    public Sprite sprite;

    public CardData(CardDataIngame dataIngame)
    {
        var data = GameConfig.Ins.GetCardData(dataIngame.id);
        id = data.id;
        sprite = data.sprite;
    }
}

[Serializable]
public class CardDataIngame
{
    public int id;
    public bool isMatched;

    public CardDataIngame(int id, bool matched)
    {
        this.id = id;
        isMatched = matched;
    }
}

[Serializable]
public class CardDatasIngame
{
    public List<CardDataIngame> data;
    public int turn;
    public int score;

    public CardDatasIngame(List<CardDataIngame> data, int turn, int score)
    {
        this.data = data;
        this.turn = turn;
        this.score = score;
    }
}
