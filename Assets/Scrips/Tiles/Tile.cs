using System.Collections.Generic;
using UnityEngine;
using UnityEngine.WSA;
using static UnityEngine.Rendering.DebugUI;

public class Tile
{
    public Vector2Int Index;

    public TileType Current;

    public TileType Next;

    public int CurrentDryness { get; set; }
    public Tile ParentTile { get; set; }
    public int SpreadRangeRemaining { get; set; }
    public Tile(TileType defaultType, Vector2Int index)
    {
        Current = defaultType;
        CurrentDryness = Current.Dryness;
        Index = index;
        SpreadRangeRemaining = Current.SpreadRange;
    }

    public void SetNext(Tile parentTile)
    {
        Next = parentTile.Current;
        CurrentDryness = Current.Dryness;
        SetParentTile(parentTile);
    }


    public void UpdateRemainingSpreadRange(TileManager controller, ref Tile[,] tileGrid)
    {
        int maxRange = Current.SpreadRange;

        for (int i = -maxRange; i < maxRange + 1; i++)
        {
            for (int j = -maxRange; j < maxRange + 1; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int newX = Index.x + i;
                int newY = Index.y + j;
                if (!controller.IsIndexValid(newX, newY))
                {
                    continue;
                }
                if (tileGrid[newX, newY].Current != Current)
                {
                    continue;
                }
                int newRemainingSpread = CalculateRemainingSpreadRange(new Vector2Int(newX, newY));
                if (tileGrid[newX, newY].SpreadRangeRemaining < newRemainingSpread)
                {
                    tileGrid[newX, newY].SpreadRangeRemaining = newRemainingSpread;
                }
            }
        }
    }
    public void ReplaceTile(Tile[,] tileGrid, Vector2Int tilePosition, TileType tileType)
    {
        Current = tileType;
        SpreadRangeRemaining = tileType.SpreadRange;
        ParentTile = tileGrid[tilePosition.x, tilePosition.y];
        CurrentDryness = Current.Dryness;
    }

    private void SetParentTile(Tile parentTile)
    {
        ParentTile = parentTile.ParentTile ?? parentTile;
        var xDelta = Index.x - ParentTile.Index.x;
        var yDelta = Index.y - ParentTile.Index.y;
        var xyDeltaSum = Mathf.Abs(xDelta) + Mathf.Abs(yDelta);
        SpreadRangeRemaining = ParentTile.Current.SpreadRange - xyDeltaSum;
    }

    private int CalculateRemainingSpreadRange(Vector2Int position)
    {
        var xDelta = Index.x - position.x;
        var yDelta = Index.y - position.y;
        var xyDeltaSum = Mathf.Abs(xDelta) + Mathf.Abs(yDelta);
        int calculatedSpread = Current.SpreadRange - xyDeltaSum;
        return calculatedSpread;
    }
}

