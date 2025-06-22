using UnityEngine;

[CreateAssetMenu(fileName = "PopulationRule", menuName = "Cellular Automata/Game Of Life/PopulationRule")]
public class PopulationRule : GameOfLifeRule
{
    [Tooltip("Range for condition (inclusive!)")]
    public Vector2Int PopulationRange = new Vector2Int(2, 3);
    public CellTypeOld TypeIfRangeFulfilled;
    public CellTypeOld TypeIfRangeUnfulFilled;

    public override void ExecuteRule(AutomataController controller, Cell[,] cellGrid, int x, int y, CellTypeOld cellType)
    {
        int livingNeighbors = CountLivingNeighbors(controller, cellGrid, x, y);
        if (PopulationRange.x <= livingNeighbors && livingNeighbors <= PopulationRange.y)
        {
            cellGrid[x, y].Next = TypeIfRangeFulfilled;
        }
        else
        {
            cellGrid[x, y].Next = TypeIfRangeUnfulFilled;
        }
    }
}
