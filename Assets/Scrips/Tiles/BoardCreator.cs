using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] private Vector2Int boardSize;
    [SerializeField] private float terrainHeight = 25;
    [SerializeField] private int randomSeed = 1;
    private TileManager tileManager;
    private TerrainBase terrainCreator;

    private void Start()
    {
        GenerateTileMap();
        GenerateEnviroment();
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
        var a = gameObject.GetComponentInChildren<TileManager>().gameObject.GetComponentsInChildren<Transform>(true).Where(t=> t.CompareTag("Tile")).ToList();
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
}