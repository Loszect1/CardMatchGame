using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameConfig : SingletonMono<GameConfig>
{
    public List<CardData> cardDatas = new();

    public CardData GetCardData(int id) => cardDatas.Find(e => e.id == id);
}
