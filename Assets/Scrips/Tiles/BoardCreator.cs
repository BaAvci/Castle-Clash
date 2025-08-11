using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Every new run this should be created!
/// </summary>
public class BoardCreator : MonoBehaviour
{
    [SerializeField] private Vector2Int boardSize;
    [SerializeField] private float terrainHeight = 25;
    [SerializeField] private int randomSeed = 1;
    [SerializeField] private GameObject player;
    [SerializeField] private List<GameObject> npcs = new List<GameObject>();

    private TileManager tileManager;
    private UnitManager unitManager;
    private TerrainBase terrainCreator;


    private void Start()
    {
        GenerateTileMap();
        GenerateEnviroment();
        InstanziateActors();
    }

    public void GenerateTileMap()
    {
        tileManager = GetComponentInChildren<TileManager>();
        tileManager.CreateGrid(boardSize);
    }

    public void GenerateEnviroment()
    {
        terrainCreator = GetComponentInChildren<TerrainSimple>();
        terrainCreator.CreateEnviroment(boardSize, terrainHeight, randomSeed);
    }

    public void GenerateBoard()
    {
        GenerateTileMap();
        GenerateEnviroment();
    }
    public void Clear()
    {
        var a = gameObject.GetComponentInChildren<TileManager>().gameObject.GetComponentsInChildren<Transform>(true).Where(t => t.CompareTag("Tile")).ToList();
        foreach (var transform in a)
        {
            DestroyImmediate(transform.gameObject);
        }
        var mesh = gameObject.GetComponentInChildren<TerrainBase>().gameObject.GetComponent<MeshFilter>();
        if (mesh != null)
        {
            mesh.sharedMesh = null;
        }
    }

    private void InstanziateActors()
    {
        CalculateCardPlayPositions(out int sectorLenght, out int spellCardsPlaySize);
        Vector2Int gridMaxUnitPlayPos = new Vector2Int(sectorLenght, boardSize.y);
        Vector2Int gridMaxSpellPlayPos = new Vector2Int(spellCardsPlaySize, boardSize.y);
        unitManager = gameObject.GetComponent<UnitManager>();
        player.GetComponent<Actor>().Instanziate(new Vector2Int(0, 0), gridMaxUnitPlayPos, gridMaxSpellPlayPos, tileManager,
           unitManager, true);
        foreach (GameObject npc in npcs)
        {
            npc.GetComponent<Actor>().Instanziate(new Vector2Int(boardSize.x, 0), gridMaxUnitPlayPos, gridMaxSpellPlayPos, tileManager, unitManager, false);
        }
    }

    private void CalculateCardPlayPositions(out int sectorLenght, out int spellCardsPlaySize)
    {
        int maxLenght = boardSize.x;
        sectorLenght = (maxLenght / 3);
        spellCardsPlaySize = maxLenght - sectorLenght;
    }
}