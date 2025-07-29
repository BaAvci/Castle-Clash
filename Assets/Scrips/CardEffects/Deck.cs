using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    /// <summary>
    /// All cards that are in the Actors deck
    /// </summary>
    public List<PlayableCard> Cards = new();

    public void PlayCard(PlayableCard card, GameObject target, Vector3 position)
    {
        var index = Cards.IndexOf(card);
        Cards[index].PlayCard(target, position);
    }

    public void AddCard(PlayableCard card, TileManager tileManager)
    {
        Cards.Add(card);
        tileManager.RegisterCardEvent(card);
    }
    public void AddCards(List<PlayableCard> cards, TileManager tileManager)
    {
        Cards.AddRange(cards);
        foreach (PlayableCard card in cards)
        {
            tileManager.RegisterCardEvent(card);
        }
    }

    public void RemoveCard(PlayableCard card, TileManager tileManager)
    {
        Cards.Remove(card);
        tileManager.UnRegisterCardEvent(card);
    }

    public void UpgradeCard(PlayableCard card)
    {
        var index = Cards.IndexOf(card);
        Cards[index].Upgrade();
    }
}
