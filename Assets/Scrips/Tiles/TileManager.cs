using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class TileManager : MonoBehaviour
{
    [SerializeField] private Vector2Int gridSize;
    [SerializeField] private float simulationInterval = 1f;
    [SerializeField] private bool simulateOnPressSpace = false;

    [Header("Tile Types")]
    [SerializeField] private TileType defaultType;
    [SerializeField] private TileData[] tileData;

    [SerializeField] private GameObject defaultTilePrefab;
    [SerializeField] private Dictionary<TileType, TileData> dictTileData = new();

    private Tile[,] tileGrid;
    private Dictionary<TileType, GameObject[,]> tileObjectPooling;
    private bool spacePressed;

    private WaitForSeconds wait;

    [SerializeField] private GameObject testUnit;

    void Start()
    {
        foreach (var tile in tileData)
        {
            dictTileData.Add(tile.TileType, tile);
        }

        tileData = tileData.OrderBy(c => c.Priority).ToArray();

        wait = new WaitForSeconds(simulationInterval);
        testUnit.GetComponent<UnitMovement>().ChangedTileCoordinates += SetTileToType;
        StartCoroutine(Co_Simulation());
    }

    public void CreateGrid(Vector2Int gridSize)
    {
        this.gridSize = gridSize;
        tileGrid = new Tile[gridSize.x, gridSize.y];
        tileObjectPooling = new();
        foreach (var tile in tileData)
        {
            tileObjectPooling[tile.TileType] = new GameObject[gridSize.x, gridSize.y];
            for (int x = 0; x < gridSize.x; x++)
            {
                for (int y = 0; y < gridSize.y; y++)
                {
                    AddTile(tile.TileType, x, y);
                }
            }
        }
    }

    private void AddTile(TileType tileType, int x, int y)
    {
        GameObject newTile = Instantiate(tileType.Prefab, new Vector3(x, 0, y), Quaternion.identity, this.transform);
        if (defaultTilePrefab != tileType.Prefab)
        {
            newTile.SetActive(false);
        }
        newTile.GetComponentInChildren<TextMeshPro>().text = $"{x},{y}";
        tileGrid[x, y] = new Tile(defaultType, new Vector2Int(x, y));
        tileObjectPooling[tileType][x, y] = newTile;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            spacePressed = true;
        }
    }

    public bool IsIndexValid(Vector2Int index)
    {
        return index.x >= 0 && index.x < gridSize.x
            && index.y >= 0 && index.y < gridSize.y;
    }

    public void SetTileToType(Vector2Int tilePosition, TileType cellType)
    {
        tileObjectPooling[tileGrid[tilePosition.x, tilePosition.y].Current][tilePosition.x, tilePosition.y].SetActive(false);
        tileGrid[tilePosition.x, tilePosition.y].Current = cellType;
        tileObjectPooling[cellType][tilePosition.x, tilePosition.y].SetActive(true);
    }

    public bool IsIndexValid(int x, int y)
    {
        return IsIndexValid(new Vector2Int(x, y));
    }

    private void CalculateNextStates()
    {
        for (int i = 0; i < tileData.Length; i++)
        {
            if (tileData[i].Priority < 0)
            {
                continue;
            }

            tileData[i].ExecuteRules(this, tileGrid);
        }
    }

    public TileData GetCellData(TileType current)
    {
        return dictTileData[current];
    }

    private IEnumerator Co_Simulation()
    {
        while (true)
        {
            if (simulateOnPressSpace)
            {
                while (!spacePressed)
                {
                    yield return null;
                }
                spacePressed = false;
            }
            else
            {
                yield return wait;
            }

            // Simulation
            CalculateNextStates();
            UpdateStates();
        }
    }

    private void UpdateStates()
    {
        Tile cell;
        for (int x = 0; x < gridSize.x; x++)
        {
            for (int y = 0; y < gridSize.y; y++)
            {
                cell = tileGrid[x, y];
                if (cell.Next != null)
                {
                    SetTileToType(new Vector2Int(x, y), cell.Next);
                    cell.Current = cell.Next;
                }

                cell.Next = null;
            }
        }
    }

    private void OnValidate()
    {
        wait = new WaitForSeconds(simulationInterval);
    }
}