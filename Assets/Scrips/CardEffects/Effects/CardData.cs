using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Used as a decopling mechanic from the scriptable object.
/// With this class, it's possible to modify 
/// </summary>
public class CardData
{
    public int ID;
    public string Name;
    public string Description;
    public Texture2D Image;
    public int Rarity; // TODO: Change to enum
    public int MaxLVL;

    public GameObject Animation;
    public GameObject Gameobject;
    public TileType TileType;

    public int Range;

    public List<ScriptableObject> ScriptableObjects = new List<ScriptableObject>();
    public List<float> MainValues = new List<float>();
    public List<float> SecondValues = new List<float>();
    public List<float> MainUpgradeValues = new List<float>();
    public List<float> SecondUpgradeValues = new List<float>();
    public CardData(SOCardData cardData)
    {
        ID = cardData.ID;
        Name = cardData.Name;
        Description = cardData.Description;
        Image = cardData.Image;
        Rarity = cardData.Rarity;
        MaxLVL = cardData.MaxLVL;
        Animation = cardData.Animation;
        Gameobject = cardData.Gameobject;
        TileType = cardData.TileType;
        Range = cardData.Range;
        ScriptableObjects.AddRange(cardData.ScriptableObjects);
        MainValues.AddRange(cardData.MainValues);
        SecondValues.AddRange(cardData.SecondValues);
        MainUpgradeValues.AddRange(cardData.MainUpgradeValues);
        SecondUpgradeValues.AddRange(cardData.SecondUpgradeValues);
    }
}
