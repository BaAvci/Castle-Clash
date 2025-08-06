using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Deck))]
[RequireComponent(typeof(CardHand))]
public class Actor : MonoBehaviour
{
    private Deck deck;
    private CardHand hand;
    // private DefaultDeck defaultDeck; (DefaultDeck is a ScriptableObject where only a list/array of cardData exists)

    private void Start()
    {
    }
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            foreach (PlayableCard card in deck.Cards)
            {
                deck.UpgradeCard(card);
            }
        }
    }

    public void Instanziate(TileManager tileManager, bool isPlayer)
    {
        CreateStartDeck(tileManager, isPlayer);
    }
    private void CreateStartDeck(TileManager tileManager, bool isPlayer)
    {
        //foreach (PlayableCard card in DefaultDeck.Cards)
        deck = GetComponent<Deck>();
        List<PlayableCard> playableCards;
        if (isPlayer)
        {
            playableCards = PlayerStartingDecks.CreateStartingDeck(1, this);
        }
        else
        {
            playableCards = EnemyStartingDecks.CreateStartingDeck(1, this);
        }
        deck.AddCards(playableCards, tileManager);

        hand = GetComponent<CardHand>();
        hand.Initialize(deck);
    }
    #region Remove this to a seperate class / function or what ever
    private static class PlayerStartingDecks
    {
        public static List<PlayableCard> CreateStartingDeck(int selectedDeck, Actor actor)
        {
            List<PlayableCard> playableCards = new();
            switch (selectedDeck)
            {
                case 1:
                    playableCards.Add(new DrowRanger(actor));
                    playableCards.Add(new DrowRanger(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    break;
                case 2:
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    break;
                case 3:
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    break;
                default:
                    break;
            }
            return playableCards;
        }

    }
    private static class EnemyStartingDecks
    {
        public static List<PlayableCard> CreateStartingDeck(int selectedDeck, Actor actor)
        {
            List<PlayableCard> playableCards = new();
            switch (selectedDeck)
            {
                case 1:
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new FireBall(actor));
                    playableCards.Add(new FireBall(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    break;
                case 2:
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    break;
                case 3:
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new Blackhole(actor));
                    break;
                default:
                    break;
            }
            return playableCards;
        }

    }
    #endregion  
}
