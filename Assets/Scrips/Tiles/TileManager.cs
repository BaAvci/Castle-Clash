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
    [SerializeField] private ElementalEffect defaultType;
    [SerializeField] private TileData[] tileData;

    [SerializeField] private GameObject defaultTilePrefab;
    [SerializeField] private Dictionary<ElementalEffect, TileData> dictTileData = new();

    private Tile[,] tileGrid;
    private bool spacePressed;

    private WaitForSeconds wait;

    [SerializeField] private GameObject testUnit;

    void Start()
    {
        foreach (var tile in tileData)
        {
            dictTileData.Add(tile.CellType, tile);
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
        tileGrid[x, y] = new Tile(defaultType, newTile, new Vector2Int(x, y));
        //tiles.Add(new Vector2Int(x, y), new TileData(newTile));
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

    public void SetTileToType(Vector2Int tilePosition, ElementalEffect cellType)
    {
        tileGrid[tilePosition.x, tilePosition.y].Current = cellType;

        if (simulateOnPressSpace)
        {
            SetMaterial(tilePosition.x, tilePosition.y);
        }
    }

    public bool IsIndexValid(int x, int y)
    {
        return IsIndexValid(new Vector2Int(x, y));
    }

    private void SetMaterial(int x, int y)
    {
        Material newMat = GetCellData(tileGrid[x, y].Current).Material;
        tileGrid[x, y].UpdateMaterial(newMat);
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

    public TileData GetCellData(ElementalEffect current)
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
                cell.Current = cell.Next ? cell.Next : cell.Current;
                cell.Next = null;

                SetMaterial(x, y);
            }
        }
    }

    private void OnValidate()
    {
        wait = new WaitForSeconds(simulationInterval);
    }
}