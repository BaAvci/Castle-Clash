using System.Collections.Generic;
using UnityEngine;

public abstract class CardData
{
    public int ID;
    [Tooltip("Name of the card")]
    public string Name;
    [Tooltip("Flavour text for the card")]
    public string Description;
    [Tooltip("All effects this card can apply to others")]
    public List<Effect> Effects;
    [Tooltip("The image that is displayed on the card")]
    public Texture2D Image;
    public int Rarity; // TODO: Change to enum
    [Tooltip("The max lvl a card can reach")]
    public int MaxLVL;
    [Tooltip("Gameobject to Spawn")]
    public GameObject GameObject;

    public CardData(int iD, string name, string description, List<Effect> effects, int rarity, int maxLVL, GameObject gameObject)
    {
        ID = iD;
        Name = name;
        Description = description;
        Effects = effects;
        Rarity = rarity;
        MaxLVL = maxLVL;
        GameObject = gameObject;
    }

    public void Play(UnitMovement unit) { }
    public abstract void Upgrade();
    public virtual void Spawn(GameObject target,GameObject spawnedObject)
    {
        if (target.TryGetComponent<UnitMovement>(out UnitMovement unitMovement))
        {
            foreach (var effect in Effects)
            {
                effect.Apply(unitMovement);
            }
        }
    }
}
