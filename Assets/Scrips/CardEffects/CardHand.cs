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
    protected int handSize;

    protected List<PlayableCard> cards = new();
    protected List<Button> cardButtons = new();

    [SerializeField] protected float drawInterval = 2;
    [SerializeField] protected GameObject testTarget;
    protected float drawTimer;
    protected int selectedCard;
    protected Actor owner;
    protected float gridOffSet = 0.5f;
    protected UnitManager unitManager;

    public abstract void SelectCard(int index);

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
    protected abstract Vector3 GetTargetPositionOfPlayedCard();
    public abstract void PlayCard();

    protected abstract void DrawCard();
    protected bool IsCardAUnit(PlayableCard playableCard)
    {
        bool isUnit = false;

        if (playableCard.CardData.Gameobject != null && playableCard.CardData.Gameobject.TryGetComponent<Unit>(out Unit _))
        {
            isUnit = true;
        }

        return isUnit;
    }

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

    protected bool IsPointWithinBounds(Vector3 point, Vector2Int min, Vector2Int max)
    {
        Vector2Int lower = Vector2Int.Min(min, max);
        Vector2Int upper = Vector2Int.Max(min, max);

        return point.x >= lower.x && point.x <= upper.x &&
               point.z >= lower.y && point.z <= upper.y;
    }
}
