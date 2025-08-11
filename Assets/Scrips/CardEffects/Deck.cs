using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    /// <summary>
    /// All cards that are in the Actors deck
    /// </summary>
    public List<PlayableCard> Cards = new();

    public void AddCard(PlayableCard card, TileManager tileManager, UnitManager unitManager)
    {
        Cards.Add(card);
        unitManager.RegisterCardEvent(card);
        tileManager.RegisterCardEvent(card);
    }
    public void AddCards(List<PlayableCard> cards, TileManager tileManager, UnitManager unitManager)
    {
        Cards.AddRange(cards);
        foreach (PlayableCard card in cards)
        {
            unitManager.RegisterCardEvent(card);
            tileManager.RegisterCardEvent(card);
        }
    }

    public void RemoveCard(PlayableCard card, TileManager tileManager, UnitManager unitManager)
    {
        Cards.Remove(card);
        tileManager.UnRegisterCardEvent(card);
        unitManager.UnRegisterCardEvent(card);
    }

    public void UpgradeCard(PlayableCard card)
    {
        var index = Cards.IndexOf(card);
        Cards[index].Upgrade();
    }
}
