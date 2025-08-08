using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AICardHand : CardHand
{
    [SerializeField] private List<PlayableCard> handCards = new();
    [SerializeField] private float playInterval = 2.5f;
    private float playTimer;
    public override void SelectCard(int index = 0)
    {
        selectedCard = UnityEngine.Random.Range(0, handSize);
    }

    protected override void Start()
    {
        owner = gameObject.GetComponent<Actor>();
    }
    protected override void Update()
    {
        playTimer += Time.deltaTime;
        if (playTimer >= playInterval && handSize > 0)
        {
            playTimer = 0;
            SelectCard();
            PlayCard();
        }
        base.Update();
    }
    protected override Vector3 GetTargetPositionOfPlayedCard()
    {
        Vector3 position = Vector3.zero;
        PlayableCard card = handCards[selectedCard];
        position = GenerateRandomPosition(IsCardAUnit(card));
        return position;
    }
    private Vector3 GenerateRandomPosition(bool isUnit)
    {
        Vector2 cardMaxPos = owner.GridMaxSpellPlayPos;
        if (isUnit)
        {
            cardMaxPos = owner.GridMaxUnitPlayPos;
        }
        float x = UnityEngine.Random.Range(MathF.Min(owner.GridStartPos.x, owner.GridMaxSpellPlayPos.x), MathF.Max(owner.GridStartPos.x, owner.GridMaxSpellPlayPos.x));
        float y = UnityEngine.Random.Range(MathF.Min(owner.GridStartPos.y, owner.GridMaxSpellPlayPos.y), MathF.Max(owner.GridStartPos.y, owner.GridMaxSpellPlayPos.y));
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

    public override void PlayCard()
    {
        PlayableCard playableCard = handCards[selectedCard];

        Vector2Int startPos = owner.GridStartPos;
        Vector2Int maxPos = owner.GridMaxSpellPlayPos;
        bool isUnit = IsCardAUnit(playableCard);

        Vector3 position = GetTargetPositionOfPlayedCard();

        if (isUnit)
        {
            maxPos = owner.GridMaxUnitPlayPos;
            position = new((int)position.x + owner.TileManager.TileSizeOffset, 1, (int)position.z);
        }

        if (!IsPointWithinBounds(position, startPos, maxPos))
        {
            return;
        }

        playableCard.Play(testTarget, position, cards);

        handCards.RemoveAt(selectedCard);

        handSize--;
        selectedCard = -1;
    }
}
