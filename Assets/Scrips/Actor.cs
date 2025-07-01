using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Deck))]
public class Actor : MonoBehaviour
{
    private Deck deck;

    private List<CardData> deckList;
    [SerializeField] private GameObject testTarget;
    [SerializeField] private GameObject testBlackHole;

    void Start()
    {
        deckList = new List<CardData>()
        {
            new BlackHole(testBlackHole)
        };
        deck = GetComponent<Deck>();
        deck.AddCards(deckList);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            foreach (CardData card in deckList)
            {
                deck.PlayCard(card, testTarget);
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            foreach (CardData card in deckList)
            {
                deck.UpgradeCard(card);
            }
        }
    }
}
