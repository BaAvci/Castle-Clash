using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Deck))]
public class Actor : MonoBehaviour
{
    private Deck deck;

    private List<PlayableCard> deckList;
    [SerializeField] private GameObject testBlackHole;
    [SerializeField] private GameObject TestCard;
    [SerializeField] private GameObject testTarget;

    void Start()
    {
        deckList = new List<PlayableCard>()
        {
            new Blackhole(),
            //new ExposingStrike(),
            //new HolyPotion(),
        };
        deck = GetComponent<Deck>();
        deck.AddCards(deckList);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))
            {
                foreach (PlayableCard card in deckList)
                {
                    deck.PlayCard(card, testTarget, hit.point);
                }
                Debug.Log(hit.point);
            }
        }
        if (Input.GetKeyDown(KeyCode.V))
        {
            foreach (PlayableCard card in deckList)
            {
                deck.UpgradeCard(card);
            }
        }
    }
}
