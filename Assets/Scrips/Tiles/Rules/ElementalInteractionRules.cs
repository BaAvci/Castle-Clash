using System.Collections.Generic;
using UnityEngine;

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

    protected List<ElementalEffect> GetNeighborTilesWithEffects(TileManager controller, Tile[,] tileGrid, int x, int y)
    {
        List<ElementalEffect> effectList = new List<ElementalEffect>();
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
                effectList.Add(tileGrid[newX, newY].Current);
            }
        }
        return effectList;
    }

    protected bool Contains(CellType[] cellTypes, CellType cellType)
    {
        for (int i = 0; i < cellTypes.Length; i++)
        {
            if (cellTypes[i] == cellType)
            {
                return true;
            }
        }
        return false;
    }

    protected float CountValuesOfNeighbors(ElementalEffect[] elementalEffects, TileManager controller, Tile[,] tileGrid, int x, int y)
    {
        float count = 0;
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
                foreach (var effect in elementalEffects)
                {
                    if (tileGrid[newX, newY].Current != effect)
                    {
                        return 0;
                    }
                    float modifier = 1;
                    if (newX == 0 || newY == 0)
                    {
                        modifier = 0.5f;
                    }
                    count += effect.cellularAutomataValue * modifier;
                }
            }
        }
        return count;
    }
}
