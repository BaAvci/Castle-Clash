using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
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
    private Camera camera;

    [SerializeField] private GameObject testUnit;

    void Awake()
    {
        foreach (var tile in tileData)
        {
            dictTileData.Add(tile.TileType, tile);
        }

        tileData = tileData.OrderBy(c => c.Priority).ToArray();

        camera = Camera.main;
        wait = new WaitForSeconds(simulationInterval);
        testUnit.GetComponent<UnitMovement>().ChangedTileCoordinates += SetTileToTypeByInteraction;
        StartCoroutine(Co_Simulation());
    }

    public void RegisterCardEvent(PlayableCard playableCard)
    {
        playableCard.CardWithTileTypePlayed += SetTileToTypeByInteraction;
        playableCard.CardWithGameObjectSpawned += RegisterUnitMovement;
    }
    public void UnRegisterCardEvent(PlayableCard playableCard)
    {
        playableCard.CardWithTileTypePlayed -= SetTileToTypeByInteraction;
        playableCard.CardWithGameObjectSpawned -= RegisterUnitMovement;
    }

    private void RegisterUnitMovement(Vector2 position, GameObject unit)
    {
        if (unit.TryGetComponent(out UnitMovement unitMovement))
        {
            unitMovement.ChangedTileCoordinates += SetTileToTypeByInteraction;
        }
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
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //{
        //    spacePressed = true;
        //}
        //if (Input.GetMouseButtonDown((int)MouseButton.Left))
        //{
        //    TestTileDataInput(tileData[6]);
        //}
        //if (Input.GetMouseButtonDown((int)MouseButton.Right))
        //{
        //    TestTileDataInput(tileData[1]);
        //}
        //if (Input.GetMouseButtonDown((int)MouseButton.Middle))
        //{
        //    TestTileDataInput(tileData[3]);
        //}
    }

    private void TestTileDataInput(TileData tileData)
    {
        Ray ray = camera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 20f))
        {
            TileManager conttoller = hit.collider.gameObject.GetComponentInParent<TileManager>();
            if (conttoller != null)
            {
                Vector2Int pos = new(Mathf.RoundToInt(hit.point.x), Mathf.RoundToInt(hit.point.z));
                conttoller.SetTileToTypeByInteraction(pos, tileData.TileType);
            }
        }
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

    private void SetTileToTypeByInteraction(Vector2 position, TileType tileType)
    {
        Vector2Int tilePosition = new Vector2Int((int)position.x, (int)position.y);
        Tile tile = tileGrid[tilePosition.x, tilePosition.y];
        tileObjectPooling[tile.Current][tilePosition.x, tilePosition.y].SetActive(false);
        tileObjectPooling[tileType][tilePosition.x, tilePosition.y].SetActive(true);
        tile.ReplaceTile(tileGrid, tilePosition, tileType);
        tile.UpdateRemainingSpreadRange(this, ref tileGrid);
    }

    private void SetTileToTypePassivly(Vector2Int tilePosition, TileType tileType)
    {
        tileObjectPooling[tileGrid[tilePosition.x, tilePosition.y].Current][tilePosition.x, tilePosition.y].SetActive(false);
        tileGrid[tilePosition.x, tilePosition.y].Current = tileType;
        tileObjectPooling[tileType][tilePosition.x, tilePosition.y].SetActive(true);
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

    private TileData GetCellData(TileType current)
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
                    SetTileToTypePassivly(new Vector2Int(x, y), cell.Next);
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