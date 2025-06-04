using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TileManager : MonoBehaviour
{
    private Dictionary<ElementalEffect, TileData> dictTiles;
    public Action<Vector2Int> UnitPositionUpdate;
    [SerializeField] private List<UnitMovement> units;
    [SerializeField] private TileData[] tileDatas;

    [SerializeField] private GameObject defaultTilePrefab;
    [SerializeField] private ElementalEffect defaultElementalEffect;
    private Vector2Int gridSize;
    private Tile[,] tileGrid;
    private TileManager tileManager;

    // TEMP
    [SerializeField] private GameObject testUnit;

    private void Awake()
    {
        dictTiles = new();
        foreach (var cell in tileDatas)
        {
            dictTiles.Add(cell.ElementalEffect, cell);
        }
        testUnit.GetComponent<UnitMovement>().ChangedTileCoordinates += CalculateNextStateUnit;
        units = new();
    }

    private void Start()
    {
        StartCoroutine(Co_CellularAutomata());
    }

    private void Update()
    {
        Debug.Log("test");
    }

    private void CalculateNextStateUnit(Vector2Int unitCoordinates, ElementalEffect[] elementalEffects)
    {
        for (int i = 0; i < tileDatas.Length; i++)
        {
            if (tileDatas[i].Priority < 0)
            {
                continue;
            }

            tileDatas[i].ExecuteRules(this, tileGrid, elementalEffects, unitCoordinates.x, unitCoordinates.y);
        }
        UpdateTile();
    }

    private void UpdateTile()
    {
        Tile tile;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                tile = tileGrid[x, y];
                tile.Current = tile.Next ? tile.Next : tile.Current;
                tile.Next = null;
                tile.Material = dictTiles[tile.Current].TileMaterial;
            }
        }
    }

    //public void AddUnits(GameObject newUnit, Vector2Int coordinates)
    //{
    //    GameObject createdUnit = Instantiate(newUnit, new Vector3(coordinates.x, 0, coordinates.y), Quaternion.identity);
    //    UnitMovement unitMovement = createdUnit.GetComponent<UnitMovement>();
    //    unitMovement.ChangedTileCoordinates += UpdateTile;
    //    units.Add(unitMovement);
    //}

    public void CreateGrid(Vector2Int gridSize)
    {
        this.gridSize = gridSize;
        tileGrid = new Tile[gridSize.x, gridSize.y];
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                AddTile(x, y);
            }
        }
    }

    private void AddTile(int x, int y)
    {
        GameObject newTile = Instantiate(defaultTilePrefab, new Vector3(x, 0, y), Quaternion.identity, this.transform);
        newTile.GetComponentInChildren<TextMeshPro>().text = $"{x},{y}";
        tileGrid[x, y] = new Tile(defaultElementalEffect, newTile);
        //tiles.Add(new Vector2Int(x, y), new TileData(newTile));
    }
    public bool IsIndexValid(Vector2Int index)
    {
        return index.x >= 0 && index.x < gridSize.x
            && index.y >= 0 && index.y < gridSize.y;
    }
    public bool IsIndexValid(int x, int y)
    {
        return IsIndexValid(new Vector2Int(x, y));
    }

    private IEnumerator Co_CellularAutomata()
    {
        UpdateTile();
        yield return 0;
    }

    private void OnDrawGizmos()
    {
        var tileSize = defaultTilePrefab.transform.localScale / 2;
        tileSize.y = 0;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero - tileSize, new Vector3(gridSize.x - 1, 0, gridSize.y - 1) + tileSize);
    }
}