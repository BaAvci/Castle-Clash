using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CardData", menuName = "Cards/CardData")]
public class SOCardData : ScriptableObject
{
    public int ID;
    [Tooltip("Name of the card")]
    public string Name;
    [Tooltip("Flavour text for the card")]
    public string Description;
    [Tooltip("The image that is displayed on the card")]
    public Texture2D Image;
    public int Rarity; // TODO: Change to enum
    [Tooltip("The max lvl a card can reach")]
    public int MaxLVL;
    [Tooltip("Animation of the Card")]
    public GameObject Animation;

    [Tooltip("Game object to spawn")]
    public GameObject Gameobject;

    public List<ScriptableObject> scriptableObjects = new List<ScriptableObject>();
    public List<float> mainValues = new List<float>();
    public List<float> SecondValues = new List<float>();
    public List<float> mainUpgradeValues = new List<float>();
    public List<float> SecondUpgradeValues = new List<float>();
}
