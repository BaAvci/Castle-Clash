using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public abstract class ElementalInteractionRules : Rule
{
    /// <summary>
    /// An effect affected tile set's all affected tiles in it's range as it's effect supplier.
    /// After that, all tiles should calculate it's own status.
    /// </summary>
    /// <param name="controller"></param>
    /// <param name="originTile"></param>
    /// <param name="tileGrid"></param>
    //protected void CalculateOmniDirectionSpread(TileManager controller, Tile originTile, Tile[,] tileGrid)
    //{
    //    Vector2Int spreadDistance = originTile.Current.spreadDistance;
    //    int maxDist = Mathf.Max(spreadDistance.x, spreadDistance.y);
    //    // offset = startPos; offset <= endPos;
    //    for (int xOffSet = 0 - spreadDistance.x; xOffSet <= 0 + spreadDistance.x; xOffSet++)
    //    {
    //        for (int yOffSet = 0 - spreadDistance.y; yOffSet <= 0 + spreadDistance.y; yOffSet++)
    //        {
    //            if (xOffSet == 0 && yOffSet == 0)
    //            {
    //                continue;
    //            }
    //            int newX = originTile.Index.x + xOffSet;
    //            int newY = originTile.Index.y + yOffSet;
    //            if (!controller.IsIndexValid(newX, newY))
    //            {
    //                continue;
    //            }
    //            int distance;
    //            if (xOffSet == 0)
    //            {
    //                distance = newY;
    //            }
    //            else if (xOffSet == 0)
    //            {
    //                distance = newX;
    //            }
    //            else
    //            {
    //                distance = Mathf.CeilToInt((newX + newY) * 0.5f);
    //            }
    //            int spreadValue = Mathf.CeilToInt(maxDist * tileGrid[newX, newX].Current.spreadDamper) - distance + 1;

    //            tileGrid[newX, newY].AddAffectingElement(originTile, spreadValue);
    //        }
    //    }
    //}
    protected List<Tile> GetNeighborTilesWithEffects(TileManager controller, Tile[,] tileGrid, int x, int y)
    {
        List<Tile> effectList = new List<Tile>();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int newX = x + i;
                int newY = y + j;
                if (!controller.IsIndexValid(newX, newY))
                {
                    continue;
                }
                if (tileGrid[newX, newY].Current.SpreadRange > 0)
                {
                    effectList.Add(tileGrid[newX, newY]);
                }
            }
        }
        return effectList;
    }

    protected List<Tile> GetNeighboursWithMatterState(TileManager controller, Tile[,] tileGrid, int x, int y, MatterState matterState)
    {
        List<Tile> tiles = new();
        for (int i = -1; i < 2; i++)
        {
            for (int j = -1; j < 2; j++)
            {
                if (i == 0 && j == 0)
                {
                    continue;
                }
                int newX = x + i;
                int newY = y + j;
                if (!controller.IsIndexValid(newX, newY))
                {
                    continue;
                }
                if (x == 0 && y == 1)
                {
                    //Debug.Log("");
                }
                Vector2Int position = new(i, j);
                int spreadControlValue;
                if (Vector2Int.left == position || Vector2Int.up == position || Vector2Int.right == position || Vector2Int.down == position)
                {
                    spreadControlValue = 1;
                }
                else
                {
                    spreadControlValue = 2;
                }
                if (tileGrid[newX, newY].Current.MatterState == matterState && tileGrid[newX, newY].SpreadRangeRemaining >= spreadControlValue)
                {
                    tiles.Add(tileGrid[newX, newY]);
                }
            }
        }
        return tiles;
    }
}
