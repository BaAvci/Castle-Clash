using System.Collections.Generic;
using System.Linq;
using UnityEditor.Rendering;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleDirectionSpread", menuName = "Cellular Automata/Rules/Spread")]
public class Spread : ElementalInteractionRules
{
    public override void ExecuteRule(TileManager controller, Tile[,] cellGrid, int x, int y, TileType tileType)
    {
        TileType newTileType = null;
        var result = GetNeighborTilesWithEffects(controller, cellGrid, x, y);
        if (x == 0 && y == 2)
        {
            Debug.Log("TEst");
        }
        if (result.Count <= 0)
        {
            return;
        }
        newTileType = SimlarMatterStateCheck(cellGrid, x, y, result, MatterState.Liquid);
        if (newTileType != null)
        {
            cellGrid[x, y].Next = newTileType;
            return;
        }
        if (cellGrid[x, y].Current.TemperatureType == TemperatureType.Normal)
        {
            newTileType = SimlarMatterStateCheck(cellGrid, x, y, result, MatterState.Plasma);
        }
        if (newTileType != null)
        {
            cellGrid[x, y].Next = newTileType;
            return;
        }

        var matterStateTiles = result.Where(t => t.Current.MatterState == MatterState.Plasma).ToList();
        int heatValue = 0;
        int coldValue = 0;
        Dictionary<TileType, int> tiletempValues = new();
        foreach (var matterStateTile in matterStateTiles)
        {
            if (!tiletempValues.TryAdd(matterStateTile.Current, matterStateTile.Current.TemperatureValue))
            {
                if (matterStateTile.Current.TemperatureType == TemperatureType.Warm)
                {
                    heatValue += matterStateTile.Current.TemperatureValue;
                }
                else if (matterStateTile.Current.TemperatureType == TemperatureType.Cold)
                {
                    coldValue += matterStateTile.Current.TemperatureValue;
                }
                tiletempValues[matterStateTile.Current] += matterStateTile.Current.TemperatureValue;
            }
        }
        int tempResult = coldValue - heatValue;
        TemperatureType tempTemperatureType = cellGrid[x, y].Current.TemperatureType;
        int drynessResult = Mathf.Abs(cellGrid[x, y].Current.Dryness) - Mathf.Abs(tempResult);
        if (tempResult < 0 && tempTemperatureType != TemperatureType.Warm)
        {
            newTileType = GetKeyOfUniqueHighestValue(tiletempValues, TemperatureType.Warm);
        }
        if (tempResult >= 0 && tempTemperatureType != TemperatureType.Cold)
        {
            newTileType = GetKeyOfUniqueHighestValue(tiletempValues, TemperatureType.Cold);
        }
        if (drynessResult > 0)
        {
            cellGrid[x, y].CalculateDryness(tempResult);
            return;
        }
        cellGrid[x, y].Next = newTileType;
    }

    private TileType SimlarMatterStateCheck(Tile[,] cellGrid, int x, int y, List<Tile> result, MatterState matterState)
    {
        TileType newTileType = null;
        var matterStateTiles = result.Where(t => t.Current.MatterState == matterState).ToList();
        if (matterStateTiles.Count > 0)
        {
            Dictionary<TileType, int> tiletempValues = new();
            foreach (var matterStateTile in matterStateTiles)
            {
                if (!tiletempValues.TryAdd(matterStateTile.Current, matterStateTile.Current.TemperatureValue))
                {
                    tiletempValues[matterStateTile.Current] += matterStateTile.Current.TemperatureValue;
                }
            }
            newTileType = GetKeyOfUniqueHighestValue(tiletempValues);
        }
        return newTileType != null ? newTileType : null;
    }

    private TileType GetKeyOfUniqueHighestValue(Dictionary<TileType, int> dict, TemperatureType temperatureType = TemperatureType.Normal)
    {
        int maxValue = 0;
        List<TileType> keyList = new List<TileType>();
        foreach (var kvp in dict)
        {
            if (kvp.Key.TemperatureType == temperatureType || temperatureType == TemperatureType.Normal)
            {
                if (kvp.Value > maxValue)
                {
                    maxValue = kvp.Value;
                    keyList.Clear();
                    keyList.Add(kvp.Key);
                }
                else if (kvp.Value == maxValue)
                {
                    keyList.Add(kvp.Key);
                }
            }
        }
        if (keyList.Count == 1)
        {
            return keyList[0];
        }
        if (temperatureType == TemperatureType.Normal)
        {
            return null;
        }
        else
        {
            return keyList[Random.Range(0, keyList.Count - 1)];
        }
    }
}
