using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TileManager : MonoBehaviour
{
    public static TileManager TileManagerInstance;
    private Dictionary<Vector2Int, TileData> tiles;
    public Action<Vector2Int> UnitPositionUpdate;
    [SerializeField] private List<UnitMovement> units;
    [SerializeField] private TileEffect[] materials;

    // TEMP
    [SerializeField] private GameObject testUnit;

    private void Awake()
    {
        if (TileManagerInstance == null)
        {
            TileManagerInstance = this;
        }
        UnitPositionUpdate = UpdateTile;
        units = new();
        tiles = new();
    }
    private void Start()
    {
        AddUnits(testUnit, new Vector2Int(0, 3));
    }

    public void UpdateTile(Vector2Int unitCoordinates)
    {
        tiles.TryGetValue(unitCoordinates, out TileData tileData);
        tileData.NextStatus();
    }

    public void AddUnits(GameObject newUnit, Vector2Int coordinates)
    {
        GameObject createdUnit = Instantiate(newUnit, new Vector3(coordinates.x, 0, coordinates.y), Quaternion.identity);
        UnitMovement unitMovement = createdUnit.GetComponent<UnitMovement>();
        unitMovement.ChangedTileCoordinates += UpdateTile;
        units.Add(unitMovement);
    }

    public void AddTile(GameObject tilePrefab, int x, int y, Transform parent)
    {
        GameObject newTile = Instantiate(tilePrefab, new Vector3(x, 0, y), Quaternion.identity, this.transform);
        newTile.GetComponentInChildren<TextMeshPro>().text = $"{x},{y}";
        tiles.Add(new Vector2Int(x, y), new TileData(newTile));
    }
}