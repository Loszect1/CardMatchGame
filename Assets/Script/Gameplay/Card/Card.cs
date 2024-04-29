using System;
using System.Collections;
using UnityEngine;

public class Card : MonoBehaviour
{
    public float rotateTime = 0.5f;

    public SpriteRenderer spriteRenderer;

    public Action<Card> onFlip = delegate { };

    bool isFlipped = false;
    public bool IsFlipped => isFlipped;

    private CardData data;
    public CardData Data => data;
    public int CardId => data.id;

    private bool isRotating = false;
    public bool IsRotating => isRotating;

    private bool matched = false;
    public bool Matched => matched;

    public void SetupCard(CardDataIngame dataIngame)
    {
        data = new(dataIngame);
        spriteRenderer.sprite = data.sprite;
        matched = dataIngame.isMatched;
    }

    public void SetupCard(CardData data)
    {
        this.data = data;
        spriteRenderer.sprite = data.sprite;
        matched = false;
    }

    private void OnMouseDown()
    {
        if (matched) return;
        Flip();
    }

    private void OnMouseEnter()
    {
        if (matched) return;
        transform.localScale = transform.localScale * 1.05f;
    }

    private void OnMouseExit()
    {
        if (matched) return;
        transform.localScale = transform.localScale / 1.05f;
    }

    private IEnumerator Rotate(Quaternion rotation, float duration)
    {
        Quaternion currentRot = transform.rotation;

        float counter = 0;

        isRotating = true;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(currentRot, rotation, counter / duration);
            yield return null;
        }

        isRotating = false;
    }

    public void Flip()
    {
        if (matched) return;
        StartCoroutine(DoFlip());
        onFlip.Invoke(this);
    }

    private IEnumerator DoFlip()
    {
        if (isRotating)
            yield return new WaitUntil(() => isRotating == false);

        if (isFlipped)
        {
            FlipBack();
        }
        else
        {
            FlipUp();
        }
    }

    public void FlipUp()
    {
        isFlipped = true;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 180, 0));
        StartCoroutine(Rotate(rotation, rotateTime));
    }

    public void FlipBack()
    {
        if (matched) return;

        isFlipped = false;
        Quaternion rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        StartCoroutine(Rotate(rotation, rotateTime));
    }

    public void MatchCard()
    {
        matched = true;
    }
}
