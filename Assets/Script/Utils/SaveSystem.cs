using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveSystem
{
    private static string path = Application.persistentDataPath + "/cardsData.json";

    public static void SaveCardsData(List<CardDataIngame> cardDatas, int turn, int score)
    {
        string savePlayerData = JsonUtility.ToJson(new CardDatasIngame(cardDatas, turn, score));
        File.WriteAllText(path, savePlayerData);
    }

    public static CardDatasIngame LoadCardsData()
    {
        if (File.Exists(path))
        {
            string loadPlayerData = File.ReadAllText(path);
            var data = JsonUtility.FromJson<CardDatasIngame>(loadPlayerData);
            return data;
        }

        return null;
    }
}
