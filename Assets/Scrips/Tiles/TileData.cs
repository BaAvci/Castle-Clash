using System.Collections.Generic;
using UnityEngine;

// Check if Cellular Automata can be done with building pattern
public enum TileStatus
{
    Normal = 0,
    Burning = 1,
    Burnt = 2,
}

public class TileData
{
    private TileStatus currentTileStatus;
    private TileStatus nextTileStatus;
    private GameObject tile;
    private TileEffect[] tileEffects;

    public TileData(GameObject tile, TileEffect[] tileEffects)
    {
        this.tile = tile;
        currentTileStatus = TileStatus.Normal;
        this.tileEffects = tileEffects;
    }

    public void NextStatus()
    {
        Material material = null;
        switch (currentTileStatus)
        {
            case TileStatus.Normal:
                tileEffects.
                break;
            case TileStatus.Burning:
                break;
            case TileStatus.Burnt:
                break;
        }
    }
}
