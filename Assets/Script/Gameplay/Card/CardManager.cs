using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class CardManager : MonoBehaviour
{
    public Card cardPrefab;
    public Transform cardContainer;

    public float minSpace = 0.01f;

    public Action<Card> onFlip = delegate { };

    private List<Card> cards;

    private float cardWidth, cardHeight;

    public void SetupCards(int row, int col, List<CardDataIngame> data)
    {
        EnsureCards(row, col);
        SetupCardsData(data);
        SetupCardsPositionAndScale(row, col);
    }

    public void SetupCards(int row, int col)
    {
        EnsureCards(row, col);
        SetupCardsData();
        SetupCardsPositionAndScale(row, col);
    }

    private void EnsureCards(int row, int col)
    {
        var cardsArr = GetComponentsInChildren<Card>();
        cards = cardsArr.ToList();

        if (cards.Count < row * col)
        {
            while (cards.Count < row * col)
            {
                var newCard = Instantiate(cardPrefab, cardContainer);
                cards.Add(newCard);
            }
        }
        else if (cards.Count > row * col)
        {
            while (cards.Count > row * col)
            {
                var card = cards[0];
                cards.Remove(card);
                if (!Application.isPlaying) DestroyImmediate(card.gameObject);
                else Destroy(card.gameObject);
            }
        }
    }

    private void SetupCardsPositionAndScale(int row, int col)
    {
        BoxCollider boxCollider = cardPrefab.GetComponent<BoxCollider>();
        if (boxCollider != null)
        {
            cardWidth = boxCollider.size.x;
            cardHeight = boxCollider.size.y;
        }

        var sprite = cardContainer.GetComponent<SpriteRenderer>();
        var containerSize = sprite.size;

        var cardScaleX = (containerSize.x - (col - 1) * minSpace) / (cardWidth * col);
        var cardScaleY = (containerSize.y - (row - 1) * minSpace) / (cardHeight * row);

        var cardScale = Math.Min(cardScaleX, cardScaleY);
        ScaleCard(cardScale);

        var firstPos = new Vector2(-containerSize.x / 2 + cardWidth / 2, containerSize.y / 2 - cardHeight / 2);
        float spaceX = cardWidth + (containerSize.x - cardWidth * col) / (col - 1);
        float spaceY = cardHeight + (containerSize.y - cardHeight * row) / (row - 1);

        for (int i = 0; i < col; i++)
        {
            for (int j = 0; j < row; j++)
            {
                int idx = i * row + j;
                cards[idx].transform.SetLocalPositionAndRotation(new Vector3(firstPos.x + i * spaceX, firstPos.y - j * spaceY, 0), Quaternion.identity);
            }
        }
    }

    private void ScaleCard(float cardScale)
    {
        foreach (var card in cards)
        {
            card.transform.localScale = Vector3.one * cardScale;
        }

        cardHeight *= cardScale;
        cardWidth *= cardScale;
    }

    private void SetupCardsData(List<CardDataIngame> datas)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            cards[i].SetupCard(datas[i]);
            cards[i].onFlip = OnFlip;
        }
    }

    private void SetupCardsData()
    {
        var listSprites = GetListRandomSprites(cards.Count / 2);

        for (int i = 0; i < cards.Count; i += 2)
        {
            var randomSprite = RandomSprite(listSprites);
            cards[i].SetupCard(randomSprite);
            cards[i].onFlip = OnFlip;
            cards[i + 1].SetupCard(randomSprite);
            cards[i + 1].onFlip = OnFlip;
            listSprites.Remove(randomSprite);
        }

        Shuffle();
    }

    private List<CardData> GetListRandomSprites(int count)
    {
        List<CardData> listSprites = new();
        List<CardData> temp = new List<CardData>(GameConfig.Ins.cardDatas);

        while (listSprites.Count < count)
        {
            var randomSprite = RandomSprite(temp);
            listSprites.Add(randomSprite);
            temp.Remove(randomSprite);
        }

        return listSprites;
    }

    private CardData RandomSprite(List<CardData> listSprites)
    {
        var rand = Random.Range(0, listSprites.Count);
        return listSprites[rand];
    }

    private void Shuffle()
    {
        for (int i = 0; i < cards.Count; i++)
        {
            var rand = Random.Range(0, cards.Count);
            var temp = cards[rand];
            cards[rand] = cards[i];
            cards[i] = temp;
        }
    }

    private void OnFlip(Card card)
    {
        onFlip.Invoke(card);
    }

    public void FlipUpAllCards()
    {
        foreach (var card in cards)
        {
            card.FlipUp();
        }
    }

    public void FlipBackAllCards()
    {
        foreach (var card in cards)
        {
            card.FlipBack();
        }
    }

    public bool AllCardMatched()
    {
        return cards.Count(e => !e.Matched) == 0;
    }

    public List<CardDataIngame> GetCardDatasIngame()
    {
        List<CardDataIngame> cardDatas = new();

        foreach (var card in cards)
        {
            cardDatas.Add(new(card.CardId, card.Matched));
        }

        return cardDatas;
    }
}
