using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Cards/CardData")]
public class SOCardData : ScriptableObject
{
    [Header("Card Description")]
    public int ID;
    [Tooltip("Name of the card")]
    public string Name;
    [Tooltip("Flavour text for the card")]
    public string Description;

    [Header("Card Visualisation")]
    [Tooltip("The image that is displayed on the card")]
    public Texture2D Image;
    public int Rarity; // TODO: Change to enum
    [Tooltip("The max lvl a card can reach")]
    public int MaxLVL;

    [Header("Card Objects")]
    [Tooltip("Animation of the Card")]
    public GameObject Animation;
    [Tooltip("Game object to spawn")]
    public GameObject Gameobject;
    [Tooltip("Sets the tiles in range to this tiletype")]
    public TileType TileType;

    public int Range;

    [Header("Card Effects")]
    public List<ScriptableObject> ScriptableObjects = new List<ScriptableObject>();
    public List<float> MainValues = new List<float>();
    public List<float> SecondValues = new List<float>();
    public List<float> MainUpgradeValues = new List<float>();
    public List<float> SecondUpgradeValues = new List<float>();
}
