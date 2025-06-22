using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "SingleDirectionSpread", menuName = "Cellular Automata/Rules/Spread")]
public class Spread : ElementalInteractionRules
{
    public override void ExecuteRule(TileManager controller, Tile[,] tileGrid, int x, int y, TileType tileType)
    {
        // Check if any liquid tiles exists, if they do only calculate them else check for plasma and solid
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
                // If there is only one possible TileType select that else select at random
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
#if UNITY_EDITOR
                else // Debug section
                {
                    Debug.LogError($"Tile with error: {new Vector2Int(x, y)}");
                    Debug.LogError($"Current Tile Data: {tileGrid[x, y].Current}");
                    Debug.LogError($"All neighbouring tiles: {neighbouringTiles.Count}");
                    Debug.LogError($"All tiles that affect current tile: {calculatedTileTemps.Count}");
                    Debug.LogError($"All possible that can actually affect current tile: {possibleTileTypes.Count}");
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
                            Debug.LogError($"Position of neighboring tile: {new Vector2Int(newX, newY)}");
                            Debug.LogError($"TielType of neighboring tile: {tileGrid[newX, newY].Current}");
                        }
                    }
                }
#endif
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
            // If there is only one option just replace it with that
            // TODO: Counter TemperatueTypes should not do that. FIRE should still take some time to take over ICE.
            if (possibleTileTypes.Count == 1)
            {
                var temp = possibleTileTypes[0];
                possibleTileTypes.Clear();
                possibleTileTypes.Add(temp);
                return;
            }
            TemperatureType tileTemp = tileGrid[x, y].Current.TemperatureType;

            int tempResult = 0;
            foreach (var item in possibleTileTypes)
            {
                tempResult += calculatedTileTemps[item];
            }
            int finalTemp = Mathf.Abs(tileGrid[x, y].CurrentDryness) - Mathf.Abs(tempResult);
            TileType result = null;
            // Somehow if i invert this if and remove the else it does not work correctly anymore. But dont care
            // I just want to move on to the next task.

            // Depending on the Temperature type of an solid tile, the same temp type gets priority over it
            switch (tileTemp)
            {
                case TemperatureType.Warm:
                    if (finalTemp < 0)
                    {
                        result = possibleTileTypes.Where(tt => tt.TemperatureType == TemperatureType.Warm).First();
                    }
                    break;
                case TemperatureType.Cold:
                    if (finalTemp >= 0)
                    {
                        result = possibleTileTypes.Where(tt => tt.TemperatureType == TemperatureType.Cold).First();
                    }
                    break;
            }
            if (result != null)
            {
                possibleTileTypes.Clear();
                possibleTileTypes.Add(result);
            }
            else
            {
                Debug.LogError("Solid and Plasma controll error.");
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
            int alteredTempValue = matterStateTile.Current.TemperatureType == TemperatureType.Warm ? matterStateTile.Current.TemperatureValue * -1 : matterStateTile.Current.TemperatureValue;
            if (!tiletempValues.TryAdd(matterStateTile.Current, alteredTempValue))
            {
                tiletempValues[matterStateTile.Current] += alteredTempValue;
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
            if (Mathf.Abs(kvp.Value) > Mathf.Abs(maxValue))
            {
                maxValue = kvp.Value;
                keyList.Clear();
                keyList.Add(kvp.Key);
            }
            else if (Mathf.Abs(kvp.Value) == Mathf.Abs(maxValue))
            {
                keyList.Add(kvp.Key);
            }
        }
        return keyList;
    }
}
