using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BoardCreator : MonoBehaviour
{
    [SerializeField] private GameObject tilePrefab;
    [SerializeField] private Vector2Int boardSize;

    void Start()
    {
        for (int x = 0; x < boardSize.x; x++)
        {
            for (int y = 0; y < boardSize.y; y++)
            {
                TileManager.TileManagerInstance.AddTile(tilePrefab, x, y, this.transform);
            }
        }
    }

    private void OnDrawGizmos()
    {
        var tileSize = tilePrefab.transform.localScale / 2;
        tileSize.y = 0;
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Vector3.zero - tileSize, new Vector3(boardSize.x - 1, 0, boardSize.y - 1) + tileSize);
    }
}