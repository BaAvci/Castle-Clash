using UnityEngine;

public enum ExecutionOrderCOPY
{
    Up,
    UpLineByLine
}

[CreateAssetMenu(fileName = "CellData", menuName = "Cellular Automata/CellData2")]
public class CellData : ScriptableObject
{
    public CellTypeOld CellType;
    public Color PixelColor = Color.black;

    public int Priority = 0;

    public RuleCopy[] Rules;
    public ExecutionOrderCOPY ExecutionOrder;

    public void ExecuteRules(AutomataController controller, Cell[,] cellGrid)
    {
        switch (ExecutionOrder)
        {
            case ExecutionOrderCOPY.Up:
                UpExecution(controller, cellGrid, Rules);
                break;
            case ExecutionOrderCOPY.UpLineByLine:
                UpLineByLineExecution(controller, cellGrid, Rules);
                break;
            default:
                break;
        }
    }

    private void UpLineByLineExecution(AutomataController controller, Cell[,] cellGrid, RuleCopy[] rules)
    {

        for (int y = 0; y < cellGrid.GetLength(1); y++)
        {
            for (int i = 0; i < rules.Length; i++)
            {
                for (int x = 0; x < cellGrid.GetLength(0); x++)
                {
                    if (cellGrid[x, y].Current != CellType)
                    {
                        continue;
                    }
                    rules[i].ExecuteRule(controller, cellGrid, x, y, CellType);
                }
            }
        }
    }

    private void UpExecution(AutomataController controller, Cell[,] cellGrid, RuleCopy[] rules)
    {
        for (int i = 0; i < rules.Length; i++)
        {
            for (int y = 0; y < cellGrid.GetLength(1); y++)
            {
                for (int x = 0; x < cellGrid.GetLength(0); x++)
                {
                    if (cellGrid[x, y].Current != CellType)
                    {
                        continue;
                    }
                    rules[i].ExecuteRule(controller, cellGrid, x, y, CellType);
                }
            }
        }
    }
}
