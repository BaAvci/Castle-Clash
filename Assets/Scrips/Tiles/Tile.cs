using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Tile
{
    public Vector2Int Index;

    public ElementalEffect Current;
#nullable enable
    public ElementalEffect? Next;
#nullable disable

    //public Dictionary<Tile, int> TileAffectingElements;

    public Material TileMaterial { get; private set; }

    public Tile(ElementalEffect defaultType, GameObject tileObject, Vector2Int index)
    {
        //TileAffectingElements = new();
        Current = defaultType;
        TileMaterial = tileObject.GetComponent<MeshRenderer>().material;
        Index = index;
    }

    public void UpdateMaterial(Material newMaterial)
    {
        TileMaterial = newMaterial;
    }

    //public void AddAffectingElement(Tile tileToAdd, int value)
    //{
    //    if (!TileAffectingElements.TryAdd(tileToAdd, value))
    //    {
    //        TileAffectingElements[tileToAdd] = value;
    //    }
    //}
    //public void RemoveAffectingElement(Tile tileToRemove)
    //{
    //    TileAffectingElements.Remove(tileToRemove);
    //}
}

