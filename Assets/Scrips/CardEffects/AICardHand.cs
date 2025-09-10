using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AICardHand : CardHand
{
    public List<PlayableCard> HandCards { get => handCards; }

    [SerializeField] private List<PlayableCard> handCards;

    protected override void Start()
    {
        handCards = new();
        owner = gameObject.GetComponent<Actor>();
    }
    public Vector3 GetTargetPositionOfPlayedCard(PlayableCard playableCard, Unit targetUnit)
    {
        Vector3 position = new Vector3(0, 0.6f, 0);
        if (targetUnit == null)
        {
            position = GenerateRandomPosition(playableCard.IsCardAUnit());
        }
        else
        {
            position = targetUnit.transform.position;
        }
        if (playableCard.IsCardAUnit())
        {
            position.x = owner.GridStartPos.x;
        }

        return position;
    }
    private Vector3 GenerateRandomPosition(bool isUnit)
    {
        Vector2 cardMaxPos = owner.GridMaxSpellPlayPos;
        if (isUnit)
        {
            cardMaxPos = owner.GridMaxUnitPlayPos;
        }
        float x = UnityEngine.Random.Range(MathF.Min(owner.GridStartPos.x, cardMaxPos.x), MathF.Max(owner.GridStartPos.x, cardMaxPos.x));
        float y = UnityEngine.Random.Range(MathF.Min(owner.GridStartPos.y, cardMaxPos.y), MathF.Max(owner.GridStartPos.y, cardMaxPos.y));
        return new Vector3(x, 0.2f, y);
    }

    protected override void DrawCard()
    {
        drawTimer += Time.deltaTime;
        if (drawTimer >= drawInterval && handSize < maxHandSize)
        {
            List<PlayableCard> drawPile = cards.Where(c => c.CardPileState == CardState.DrawPile).ToList();
            if (drawPile.Count == 0)
            {
                ShuffleCards();
                return;
            }
            drawTimer = 0;
            int cardIndex = UnityEngine.Random.Range(0, drawPile.Count - 1);
            PlayableCard card = drawPile[cardIndex];
            card.Draw(cards);

            handSize++;

            handCards.Add(card);
        }
    }

    protected override void RemoveCardFromSelection(PlayableCard playableCard)
    {
        handCards.Remove(playableCard);
    }
}
