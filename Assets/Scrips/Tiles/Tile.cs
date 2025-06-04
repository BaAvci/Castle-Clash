using UnityEngine;

public class Tile
{
    public Vector2Int Index;
    public ElementalEffect Current;
    public ElementalEffect? Next;
    public GameObject GameObject;
    public Material Material;
    public Tile(ElementalEffect defaultType, GameObject gameObject)
    {
        Current = defaultType;
        GameObject = gameObject;
        Material = GameObject.GetComponentInChildren<MeshRenderer>().material;
    }
}

