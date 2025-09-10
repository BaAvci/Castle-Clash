using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using UnityEngine.Rendering;
using UnityEngine.VFX;

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

    void Awake()
    {
        foreach (var tile in tileData)
        {
            dictTileData.Add(tile.TileType, tile);
        }

        tileData = tileData.OrderBy(c => c.Priority).ToArray();

        camera = Camera.main;
        wait = new WaitForSeconds(simulationInterval);
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

    private void RegisterUnitMovement(Vector3 position, GameObject unit)
    {
        UnitMovement unitMovement = unit.GetComponentInChildren<UnitMovement>(true);
        unitMovement.UnitTileEffectChange += SetTileToTypeByInteraction;
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
            TileManager controller = hit.collider.gameObject.GetComponentInParent<TileManager>();
            if (controller != null)
            {
                Vector3Int pos = new(Mathf.RoundToInt(hit.point.x),0, Mathf.RoundToInt(hit.point.z));
                controller.SetTileToTypeByInteraction(tileData.TileType, pos);
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

    private void SetTileToTypeByInteraction(TileType tileType, Vector3 position)
    {
        Vector2Int tilePosition = new Vector2Int((int)position.x, (int)position.z);
        if (!IsIndexValid(tilePosition))
        {
            return;
        }
        Tile tile = tileGrid[tilePosition.x, tilePosition.y];
        GameObject oldTile = tileObjectPooling[tile.Current][tilePosition.x, tilePosition.y];
        GameObject newTile = tileObjectPooling[tileType][tilePosition.x, tilePosition.y];
        oldTile.SetActive(false);
        newTile.SetActive(true);
        VFXLerp(oldTile, newTile);
        tile.ReplaceTile(tileGrid, tilePosition, tileType);
        tile.UpdateRemainingSpreadRange(this, ref tileGrid);
    }

    // Tried to slowly change from one grass texture to another but failed misserable
    private void VFXLerp(GameObject oldTile, GameObject newTile)
    {
        if (newTile.TryGetComponent<VisualEffect>(out VisualEffect effect))
        {
            float time = 0f;
            effect.SetVector4("MainColor", oldTile.GetComponentInChildren<MeshRenderer>().sharedMaterial.color);
            effect.SetVector4("TargetColor", newTile.GetComponentInChildren<MeshRenderer>().sharedMaterial.color);
            effect.SetFloat("Time", time);
            StartCoroutine(Co_Lerp(effect));
        }
    }
    private IEnumerator Co_Lerp(VisualEffect effect)
    {
        float time = 0f;
        while (time < 5f)
        {
            effect.SetFloat("Time", time);
            time += Time.deltaTime;
        }
        effect.SetFloat("Time", time);
        yield return null;
    }
    private void SetTileToTypePassively(Vector2Int tilePosition, TileType tileType)
    {
        GameObject oldTile = tileObjectPooling[tileGrid[tilePosition.x, tilePosition.y].Current][tilePosition.x, tilePosition.y];
        oldTile.SetActive(false);
        tileGrid[tilePosition.x, tilePosition.y].Current = tileType;

        GameObject newTile = tileObjectPooling[tileType][tilePosition.x, tilePosition.y];
        newTile.SetActive(true);
        VFXLerp(oldTile, newTile);
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
                    SetTileToTypePassively(new Vector2Int(x, y), cell.Next);
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