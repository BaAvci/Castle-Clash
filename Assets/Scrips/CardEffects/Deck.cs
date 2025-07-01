using System.Collections.Generic;
using UnityEngine;

public class Deck : MonoBehaviour
{
    public List<CardData> Cards = new();

    public void PlayCard(CardData card, GameObject target)
    {
        var index = Cards.IndexOf(card);
        GameObject objectToCreate = Cards[index].GameObject;
        GameObject createdObject = Instantiate(objectToCreate, target.transform.position, Quaternion.identity);
        Cards[index].Spawn(target, createdObject);
    }

    public void AddCard(CardData card)
    {
        Cards.Add(card);
    }
    public void AddCards(List<CardData> cards)
    {
        Cards.AddRange(cards);
    }

    public void RemoveCard(CardData card)
    {
        Cards.Remove(card);
    }

    public void UpgradeCard(CardData card)
    {
        var index = Cards.IndexOf(card);
        Cards[index].Upgrade();
    }
}
