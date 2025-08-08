using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Deck))]
[RequireComponent(typeof(CardHand))]
public class Actor : MonoBehaviour
{
    private Deck deck;
    private CardHand hand;
    public Vector2Int GridStartPos { get; private set; }
    public Vector2Int GridMaxUnitPlayPos { get; private set; }
    public Vector2Int GridMaxSpellPlayPos { get; private set; }
    public TileManager TileManager { get; private set; }
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

    public void Instanziate(Vector2Int gridStartPos, Vector2Int gridMaxUnitPlayPos, Vector2Int gridMaxSpellPlayPos, TileManager tileManager, bool isPlayer)
    {
        GridStartPos = gridStartPos;
        if (gameObject.CompareTag("Enemy"))
        {
            gridMaxUnitPlayPos.x = gridStartPos.x - gridMaxUnitPlayPos.x;
            gridMaxSpellPlayPos.x = gridStartPos.x - gridMaxSpellPlayPos.x;
        }
        GridMaxUnitPlayPos = gridMaxUnitPlayPos;
        GridMaxSpellPlayPos = gridMaxSpellPlayPos;
        TileManager = tileManager;
        CreateStartDeck(isPlayer);
    }
    private void CreateStartDeck(bool isPlayer)
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
        deck.AddCards(playableCards, TileManager);

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
                    playableCards.Add(new Blackhole(actor));
                    playableCards.Add(new DrowRanger(actor));
                    playableCards.Add(new DrowRanger(actor));
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
