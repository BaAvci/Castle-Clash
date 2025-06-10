using NUnit.Framework.Constraints;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public enum CellType
{
    // Game of Life
    Dead = 0,
    Alive = 1,

    // Sand Simulation
    Air = 2,
    Sand = 3,
    Water = 4,
    Obstacle = 5,
}

public class Cell
{
    public Vector2Int Index;

    public CellType Current;
    public CellType? Next;

    public Cell(CellType defaultType)
    {
        Current = defaultType;
    }
}

public class AutomataController : MonoBehaviour
{
    [SerializeField] private Vector2Int dimensions = new(8, 8);
    [SerializeField] private float simulationInterval = 1f;
    [SerializeField] private bool simulateOnPressSpace = false;

    [Header("Cell Types")]
    [SerializeField] private CellType defaultType = CellType.Dead;
    [SerializeField] private CellData[] cellData;

    private Texture2D texture;
    private Cell[,] cellGrid;
    private bool spacePressed;

    private WaitForSeconds wait;
    private Dictionary<CellType, CellData> dictCellDate = new();

    void Start()
    {
        GetComponent<MeshRenderer>().material.mainTexture = texture = new Texture2D(dimensions.x, dimensions.y);
        texture.filterMode = FilterMode.Point;

        foreach (var cell in cellData)
        {
            dictCellDate.Add(cell.CellType, cell);
        }
        cellGrid = new Cell[dimensions.x, dimensions.y];
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                cellGrid[x, y] = new Cell(defaultType);
                SetPixel(x, y);
            }
        }
        texture.Apply();

        cellData = cellData.OrderBy(c => c.Priority).ToArray();

        wait = new WaitForSeconds(simulationInterval);

        StartCoroutine(Co_Simulation());
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
        return index.x >= 0 && index.x < dimensions.x
            && index.y >= 0 && index.y < dimensions.y;
    }

    public void SetPixelToType(int x, int y, CellType cellType)
    {
        cellGrid[x, y].Current = cellType;

        if (simulateOnPressSpace)
        {
            SetPixel(x, y);
            texture.Apply();
        }
    }

    //The Method that takes the input and changes the pixel
    public void SetPixelToType(Vector3 position, CellType cellType)
    {
        Vector3 bottomLeft = transform.position + new Vector3(-5 * transform.localScale.x, -5 * transform.localScale.y);
        float deltaX = position.x - bottomLeft.x;
        float deltaY = position.y - bottomLeft.y;

        float pixelSizeX = 10f * transform.localScale.x / dimensions.x;
        float pixelSizeY = 10f * transform.localScale.y / dimensions.y;

        int x = Mathf.FloorToInt(Mathf.Clamp(deltaX / pixelSizeX, 0, dimensions.x - 1));
        int y = Mathf.FloorToInt(Mathf.Clamp(deltaY / pixelSizeY, 0, dimensions.y - 1));

        SetPixelToType(x, y, cellType);
    }

    public bool IsIndexValid(int x, int y)
    {
        return IsIndexValid(new Vector2Int(x, y));
    }

    private void SetPixel(int x, int y)
    {
        Color color = GetCellData(cellGrid[x, y].Current).PixelColor;
        texture.SetPixel(x, y, color);
    }

    private void CalculateNextStates()
    {
        for (int i = 0; i < cellData.Length; i++)
        {
            if (cellData[i].Priority < 0)
            {
                continue;
            }

            cellData[i].ExecuteRules(this, cellGrid);
        }
    }

    public CellData GetCellData(CellType current)
    {
        return dictCellDate[current];
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
        Cell cell;
        for (int x = 0; x < dimensions.x; x++)
        {
            for (int y = 0; y < dimensions.y; y++)
            {
                cell = cellGrid[x, y];
                cell.Current = cell.Next.HasValue ? cell.Next.Value : cell.Current;
                cell.Next = null;

                SetPixel(x, y);
            }
        }
        texture.Apply();
    }

    private void OnValidate()
    {
        wait = new WaitForSeconds(simulationInterval);
    }
}
