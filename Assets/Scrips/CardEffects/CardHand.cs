using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class CardHand : MonoBehaviour
{
    private int maxHandSize = 10;
    private int handSize;

    private Deck drawPile = new();
    private Deck discardPile = new();
    private List<Button> cardButtons = new();
    [SerializeField] GameObject testTarget;

    private void Start()
    {
        for (int i = 0; i < maxHandSize; i++)
        {
            //Instantiate(cardButtons[i]);
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                foreach (PlayableCard card in drawPile.Cards)
                {
                    card.PlayCard(testTarget, hit.point);
                }
                Debug.LogWarning(hit.point);
            }
        }
    }

    public void Initialize(Deck actorDeck)
    {
        drawPile = actorDeck;
    }
}
