using UnityEngine;

public enum ExecutionOrder
{
    Up,
    UpLineByLine
}

[CreateAssetMenu(fileName = "CellData", menuName = "Cellular Automata/Tile Data")]
public class TileData : ScriptableObject
{
    public TileType TileType;

    public int Priority = 0;

    public Rule[] Rules;
    public ExecutionOrder ExecutionOrder;

    public void ExecuteRules(TileManager controller, Tile[,] cellGrid)
    {
        switch (ExecutionOrder)
        {
            case ExecutionOrder.Up:
                UpExecution(controller, cellGrid, Rules);
                break;
            case ExecutionOrder.UpLineByLine:
                UpLineByLineExecution(controller, cellGrid, Rules);
                break;
            default:
                break;
        }
    }

    private void UpLineByLineExecution(TileManager controller, Tile[,] cellGrid, Rule[] rules)
    {

        for (int y = 0; y < cellGrid.GetLength(1); y++)
        {
            for (int i = 0; i < rules.Length; i++)
            {
                for (int x = 0; x < cellGrid.GetLength(0); x++)
                {
                    rules[i].ExecuteRule(controller, cellGrid, x, y, TileType);
                }
            }
        }
    }

    private void UpExecution(TileManager controller, Tile[,] cellGrid, Rule[] rules)
    {
        for (int i = 0; i < rules.Length; i++)
        {
            for (int y = 0; y < cellGrid.GetLength(1); y++)
            {
                for (int x = 0; x < cellGrid.GetLength(0); x++)
                {
                    if (cellGrid[x, y].Current != TileType)
                    {
                        continue;
                    }
                    rules[i].ExecuteRule(controller, cellGrid, x, y, TileType);
                }
            }
        }
    }
}
