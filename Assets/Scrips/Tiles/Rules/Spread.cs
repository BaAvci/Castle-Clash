using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleDirectionSpread", menuName = "Cellular Automata/Rules/Spread")]
public class Spread : ElementalInteractionRules
{
    public override void ExecuteRule(TileManager controller, Tile[,] tileGrid, int x, int y, TileType tileType)
    {
        List<Tile> neighbouringTiles = CalculateTilechange(controller, tileGrid, x, y, MatterState.Liquid);
        if (tileGrid[x, y].Next != null || neighbouringTiles.Count > 0 || (neighbouringTiles.Count == 1 && tileGrid[x, y] == neighbouringTiles[0]))
        {
            return;
        }

        CalculateTilechange(controller, tileGrid, x, y, MatterState.Plasma);
    }

    private List<Tile> CalculateTilechange(TileManager controller, Tile[,] tileGrid, int x, int y, MatterState matterState)
    {
        MatterState selectedTileMatterState = tileGrid[x, y].Current.MatterState;
        List<Tile> neighbouringTiles;
        Dictionary<TileType, int> calculatedTileTemps;

        //Tiletypes that can be the current tiles next type.
        List<TileType> possibleTileTypes;

        if (new Vector2Int(0, 1) == tileGrid[x, y].Index)
        {
            Debug.Log("");
        }

        neighbouringTiles = GetNeighboursWithMatterState(controller, tileGrid, x, y, matterState);

        if (selectedTileMatterState == matterState)
        {
            neighbouringTiles.Add(tileGrid[x, y]);
        }
        if (neighbouringTiles.Count > 0)
        {
            calculatedTileTemps = CalculateMatterValues(neighbouringTiles);
            possibleTileTypes = GetKeyOfUniqueHighestValue(calculatedTileTemps);

            SelfTileSolidTempValueCheck(tileGrid, x, y, selectedTileMatterState, calculatedTileTemps, possibleTileTypes);

            if (!possibleTileTypes.Contains(tileGrid[x, y].Current))
            {
                if (possibleTileTypes.Count == 1)
                {
                    var partenTile = neighbouringTiles.Where(t => t.Current == possibleTileTypes[0]).First();
                    tileGrid[x, y].SetNext(partenTile);
                }
                else if (possibleTileTypes.Count > 1)
                {
                    var nextType = possibleTileTypes[Random.Range(0, possibleTileTypes.Count - 1)];
                    var partenTile = neighbouringTiles.Where(t => t.Current == nextType).First();
                    tileGrid[x, y].SetNext(partenTile);
                }
                else
                {
                    Debug.LogError("Something bad happend!");
                }
            }
        }
        return neighbouringTiles;
    }

    /// <summary>
    /// Checks if the current tile is solid and is not normal temptype.
    /// EX: If it's ICE and there is FIRE around it should not convert to FIRE in an instand.
    /// Rather it should do so after it has overcome the "Dryness" value of the ICE
    /// </summary>
    /// <param name="tileGrid"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="selectedTileMatterState"></param>
    /// <param name="calculatedTileTemps"></param>
    /// <param name="possibleTileTypes"></param>
    private static void SelfTileSolidTempValueCheck(Tile[,] tileGrid, int x, int y, MatterState selectedTileMatterState, Dictionary<TileType, int> calculatedTileTemps, List<TileType> possibleTileTypes)
    {
        if (selectedTileMatterState == MatterState.Solid && tileGrid[x, y].Current.TemperatureType != TemperatureType.Normal)
        {
            if (possibleTileTypes.Count == 1 && possibleTileTypes[0].TemperatureType == tileGrid[x, y].Current.TemperatureType)
            {
                var temp = possibleTileTypes[0];
                possibleTileTypes.Clear();
                possibleTileTypes.Add(temp);
                return;
            }

            var tempResult = tileGrid[x, y].CurrentDryness;
            foreach (var item in possibleTileTypes)
            {
                tempResult += calculatedTileTemps[item];
            }
            if (Mathf.Abs(tileGrid[x, y].CurrentDryness) - Mathf.Abs(tempResult) <= 0)
            {
                var tempType = tileGrid[x, y].Current.TemperatureType == TemperatureType.Warm ? TemperatureType.Cold : TemperatureType.Warm;
                var result = possibleTileTypes.Where(tt => tt.TemperatureType == tempType).First();
                possibleTileTypes.Clear();
                possibleTileTypes.Add(result);
            }
        }
    }

    /// <summary>
    /// Gets all TileTypes from a list of tiles and calculates all temperatues of all tiletypes.
    /// </summary>
    /// <param name="result">List of all tiles that should be checked</param>
    /// <param name="matterState">The matter state that should be looked for</param>
    /// <returns>TileType that had the highest absolut temperature value</returns>
    private Dictionary<TileType, int> CalculateMatterValues(List<Tile> result)
    {
        Dictionary<TileType, int> tiletempValues = new();
        foreach (var matterStateTile in result)
        {
            if (matterStateTile.SpreadRangeRemaining <= 0)
            {
                continue;
            }
            if (!tiletempValues.TryAdd(matterStateTile.Current, matterStateTile.Current.TemperatureValue))
            {
                int tempValue = matterStateTile.Current.TemperatureValue;
                if (matterStateTile.Current.TemperatureType == TemperatureType.Warm)
                {
                    tempValue *= -1;
                }
                tiletempValues[matterStateTile.Current] += tempValue;
            }
        }
        return tiletempValues;
    }

    /// <summary>
    /// Retruns the TileType with the highest Temp value from a dictionary.
    /// </summary>
    /// <param name="dict"></param>
    /// <param name="temperatureType"></param>
    /// <returns></returns>
    private List<TileType> GetKeyOfUniqueHighestValue(Dictionary<TileType, int> dict)
    {
        int maxValue = 0;

        List<TileType> keyList = new List<TileType>();
        foreach (var kvp in dict)
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
        return keyList;
    }
}
