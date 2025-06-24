using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] private Vector2Int boardSize;
    [SerializeField] private float terrainHeight = 25;
    private TileManager tileManager;
    private TerrainBase terrainCreator;

    private void Start()
    {
        tileManager = GetComponentInChildren<TileManager>();
        tileManager.CreateGrid(boardSize);
        terrainCreator = GetComponentInChildren<TerrainSimple>();
        terrainCreator.CreateEnviroment(boardSize, terrainHeight);
    }
}