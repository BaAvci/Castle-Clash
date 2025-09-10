using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Deck))]
[RequireComponent(typeof(CardHand))]
public class Actor : MonoBehaviour
{
    public Vector2Int GridStartPos { get; private set; }
    public Vector2Int GridMaxUnitPlayPos { get; private set; }
    public Vector2Int GridMaxSpellPlayPos { get; private set; }
    public TileManager TileManager { get; private set; }
    private Deck deck;
    private CardHand hand;
    private UnitManager unitManager;

    // Move this section to another class
    [SerializeField] private float exposedHP;
    public float HealthPoints = 30; // TODO: Move to a seperate class after AI implementation.
    public event Action<Actor, float> HealthChanged;
    public bool IsPlayer;

    public void ReceiveDamage()
    {
        HealthPoints--;
        exposedHP = HealthPoints;
        HealthChanged?.Invoke(this, HealthPoints);
        if (HealthPoints <= 0)
        {
            Debug.Log($"{gameObject.name} is dead!");
        }
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

    public void Instanziate(Vector2Int gridStartPos, Vector2Int gridMaxUnitPlayPos, Vector2Int gridMaxSpellPlayPos, TileManager tileManager, UnitManager unitManager, bool isPlayer)
    {
        GridStartPos = gridStartPos;
        IsPlayer = isPlayer;
        if (gameObject.CompareTag("Enemy"))
        {
            gridMaxUnitPlayPos.x = gridStartPos.x - gridMaxUnitPlayPos.x;
            gridMaxSpellPlayPos.x = gridStartPos.x - gridMaxSpellPlayPos.x;
        }
        GridMaxUnitPlayPos = gridMaxUnitPlayPos;
        GridMaxSpellPlayPos = gridMaxSpellPlayPos;
        TileManager = tileManager;
        this.unitManager = unitManager;
        CreateStartDeck(isPlayer);
    }
    private void OnValidate()
    {
        if (exposedHP != HealthPoints)
        {
            HealthPoints = exposedHP;
            HealthChanged?.Invoke(this, HealthPoints);
        }
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
        deck.AddCards(playableCards, TileManager, unitManager);

        hand = GetComponent<CardHand>();
        hand.Initialize(deck, unitManager);
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
                    playableCards.Add(new DragonKnight(actor));
                    playableCards.Add(new DragonKnight(actor));
                    playableCards.Add(new ThunderStrike(actor));
                    playableCards.Add(new ThunderStrike(actor));
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
                    playableCards.Add(new DragonKnight(actor));
                    playableCards.Add(new DragonKnight(actor));
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
