using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCardHand : CardHand
{
    private Dictionary<GameObject, PlayableCard> handCards = new();
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private GridLayoutGroup layoutGroup;

    public override void SelectCard(int index)
    {
        selectedCard = index;
    }
    protected override void Start()
    {
        owner = gameObject.GetComponent<Actor>();
        for (int i = 0; i < maxHandSize; i++)
        {
            GameObject card = Instantiate(buttonPrefab, layoutGroup.transform);
            var a = i;
            card.GetComponent<Button>().onClick.AddListener(() =>
            {
                SelectCard(a);
            });
            handCards.Add(card, null);
        }
        selectedCard = -1;
    }

    protected override void Update()
    {
        if (Input.GetMouseButtonDown(0) && selectedCard > -1)
        {
            PlayCard();
        }
        if (Input.GetMouseButtonDown((int)Unity.VisualScripting.MouseButton.Right))
        {
            selectedCard = -1;
        }
        base.Update();
    }

    protected override Vector3 GetTargetPositionOfPlayedCard()
    {
        Vector3 position = Vector3.zero;
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit = new();

        if (Physics.Raycast(ray, out hit))
        {
            position = hit.point;
        }
        position.y = 0.2f;
        return position;
    }

    protected override void DrawCard()
    {
        drawTimer += Time.deltaTime;
        if (drawTimer >= drawInterval && handSize < maxHandSize)
        {
            drawTimer = 0;
            List<PlayableCard> drawPile = cards.Where(c => c.CardPileState == CardState.DrawPile).ToList();
            if (drawPile.Count == 0)
            {
                ShuffleCards();
                return;
            }
            int cardIndex = UnityEngine.Random.Range(0, drawPile.Count - 1);
            PlayableCard card = drawPile[cardIndex];
            card.Draw(cards);

            handSize++;

            GameObject selectedCard = handCards.Keys.First(k => !k.activeSelf);
            FillCard(card, selectedCard);
            selectedCard.SetActive(true);
            handCards[selectedCard] = card;
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

    public override void PlayCard()
    {
        GameObject card = handCards.ElementAt(selectedCard).Key;
        PlayableCard playableCard = handCards[card];

        Vector2Int startPos = owner.GridStartPos;
        Vector2Int maxPos = owner.GridMaxSpellPlayPos;
        bool isUnit = IsCardAUnit(playableCard);

        Vector3 position = GetTargetPositionOfPlayedCard();
        List<Unit> affectedUnits = new();

        if (isUnit)
        {
            maxPos = owner.GridMaxUnitPlayPos;
            position = new((int)position.x + owner.TileManager.TileSizeOffset, 1, (int)position.z);
        }
        else
        {
            affectedUnits = unitManager.GetAllUnitsInRange(position, playableCard.CardData.Range);
        }

        if (!IsPointWithinBounds(position, startPos, maxPos))
        {
            return;
        }

        playableCard.Play(affectedUnits, position, cards);

        handCards[card] = null;
        card.SetActive(false);

        handSize--;
        selectedCard = -1;
    }
}
