using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<PlayableCard> Cards = new();

    public void PlayCard(PlayableCard card, GameObject target,Vector3 position)
    {
        var index = Cards.IndexOf(card);
        Cards[index].ApplyEffects(target);
        Cards[index].SpawnGameObject(position);
    }

    public void AddCard(PlayableCard card)
    {
        Cards.Add(card);
    }
    public void AddCards(List<PlayableCard> cards)
    {
        Cards.AddRange(cards);
    }

    public void RemoveCard(PlayableCard card)
    {
        Cards.Remove(card);
    }

    public void UpgradeCard(PlayableCard card)
    {
        var index = Cards.IndexOf(card);
        Cards[index].Upgrade();
    }
}
