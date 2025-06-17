using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;

public class Tile
{
    public Vector2Int Index;

    public TileType Current;
#nullable enable
    public TileType? Next;
#nullable disable
    public int CurrentDryness { get; private set; }

    public Tile(TileType defaultType, Vector2Int index)
    {
        Current = defaultType;
        CurrentDryness = Current.Dryness;
        Index = index;
    }

    public void CalculateDryness(int dryness)
    {
        CurrentDryness += dryness;
    }
}

