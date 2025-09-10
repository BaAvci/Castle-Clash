using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEditor.PlayerSettings;
using static UnityEngine.UI.CanvasScaler;

public abstract class CardHand : MonoBehaviour
{
    protected int maxHandSize = 10;
    [SerializeField] protected int handSize;

    protected List<PlayableCard> cards = new();
    protected List<Button> cardButtons = new();

    [SerializeField] protected float drawInterval = 2;
    protected float drawTimer;
    protected int selectedCard;
    protected Actor owner;
    protected float gridOffSet = 0.5f;
    protected UnitManager unitManager;

    public void Initialize(Deck actorDeck, UnitManager unitManager)
    {
        this.unitManager = unitManager;
        cards = actorDeck.Cards;
        foreach (PlayableCard card in cards)
        {
            card.ResetCardState();
        }
    }

    protected abstract void Start();

    protected virtual void Update()
    {
        DrawCard();
    }
    public void PlayCard(PlayableCard playableCard, Vector3 position)
    {
        Vector2Int startPos = owner.GridStartPos;
        Vector2Int maxPos = owner.GridMaxSpellPlayPos;
        bool isUnit = playableCard.IsCardAUnit();

        List<Unit> affectedUnits = new();

        if (isUnit)
        {
            maxPos = owner.GridMaxUnitPlayPos;
            int roundedValue = Mathf.RoundToInt(position.x);
            position = new(roundedValue, 0, (int)position.z);
        }
        else
        {
            affectedUnits = unitManager.GetAllUnitsInRange(position, playableCard.CardData.Range);
        }

        if (!IsPointWithinBounds(position, startPos, maxPos))
        {
            return;
        }

        playableCard.Play(affectedUnits, position, cards);

        handSize--;
        selectedCard = -1;
        RemoveCardFromSelection(playableCard);
    }

    protected abstract void DrawCard();

    protected void ShuffleCards()
    {

        List<PlayableCard> discardPile = new();
        foreach (PlayableCard card in cards)
        {
            if (card.CardPileState == CardState.DiscardPile)
            {
                discardPile.Add(card);
            }
        }
        for (int i = 0; i < discardPile.Count; i++)
        {
            discardPile[i].Shuffle(cards);
        }
    }
    protected abstract void RemoveCardFromSelection(PlayableCard playableCard);
    protected bool IsPointWithinBounds(Vector3 point, Vector2Int min, Vector2Int max)
    {
        Vector2Int lower = Vector2Int.Min(min, max);
        Vector2Int upper = Vector2Int.Max(min, max);

        return point.x >= lower.x && point.x <= upper.x &&
               point.z >= lower.y && point.z <= upper.y;
    }
}
