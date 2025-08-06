using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardHand : MonoBehaviour
{
    private int maxHandSize = 10;
    private int handSize;

    private List<PlayableCard> cards = new();
    private List<Button> cardButtons = new();
    private Dictionary<GameObject, PlayableCard> handCards = new();
    [SerializeField] GameObject testTarget;
    [SerializeField] GameObject buttonPrefab;
    [SerializeField] private float drawInterval = 2;
    [SerializeField] private GridLayoutGroup layoutGroup;
    private float drawTimer;
    private int selectedCard;

    private void Start()
    {
        Debug.Log(gameObject.name);
        for (int i = 0; i < maxHandSize; i++)
        {
            GameObject card = Instantiate(buttonPrefab, layoutGroup.transform);
            var a = i;
            card.GetComponent<Button>().onClick.AddListener(() => { SelectCard(a); });
            handCards.Add(card, null);
        }
        selectedCard = -1;
    }

    private void Update()
    {
        if (gameObject.CompareTag("Player"))
        {
            if (Input.GetMouseButtonDown(0) && selectedCard > -1)
            {
                PlayCard();
            }
            if (Input.GetMouseButtonDown((int)MouseButton.Right))
            {
                selectedCard = -1;
            }
            DrawCard();
        }
    }
    public void PlayCard()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new();

        if (Physics.Raycast(ray, out hit))
        {
            GameObject card = handCards.ElementAt(selectedCard).Key;
            PlayableCard playableCard = handCards[card];
            playableCard.Play(testTarget, hit.point, cards);

            handCards[card] = null;
            card.SetActive(false);

            Debug.LogWarning(hit.point);
            handSize--;
            selectedCard = -1;
        }
    }

    private void DrawCard()
    {
        drawTimer += Time.deltaTime;
        if (drawTimer >= drawInterval && handSize < maxHandSize)
        {
            drawTimer = 0;
            List<PlayableCard> drawPile = cards.Where(c => c.CardPileState == CardState.DrawPile).ToList();
            if (drawPile.Count == 0)
            {
                ShuffleCards();
            }
            int cardIndex = UnityEngine.Random.Range(0, drawPile.Count - 1);
            PlayableCard card = drawPile[cardIndex];
            card.Draw(cards);

            handSize++;
            GameObject selectedCard = handCards.Keys.First(k => !k.activeSelf);
            FillCard(card, selectedCard);
            handCards[selectedCard] = card;
            selectedCard.SetActive(true);
        }
    }

    private void FillCard(PlayableCard card, GameObject selectedCard)
    {
        foreach (Transform transform in selectedCard.transform)
        {
            if (transform.CompareTag("CardTitle"))
            {
                transform.gameObject.GetComponent<TextMeshProUGUI>().SetText(card.CardData.Name);
            }
            if (transform.CompareTag("CardImage"))
            {
                //transform.gameObject.GetComponent<RawImage>().texture = card.CardData.Image;
            }
            if (transform.CompareTag("CardText"))
            {
                transform.gameObject.GetComponent<TextMeshProUGUI>().SetText(card.CardData.Description);
            }
        }
    }

    public void SelectCard(int index)
    {
        selectedCard = index;
    }

    private void ShuffleCards()
    {
        List<PlayableCard> discardPile = cards.Where(c => c.CardPileState == CardState.DrawPile).ToList();
        for (int i = 0; i < discardPile.Count; i++)
        {
            discardPile[i].Shuffle(cards);
        }
    }
    public void Initialize(Deck actorDeck)
    {
        cards = actorDeck.Cards;
        foreach (PlayableCard card in cards)
        {
            card.ResetCardState();
        }
    }
}
