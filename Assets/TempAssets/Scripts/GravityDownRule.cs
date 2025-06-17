using UnityEngine;

[CreateAssetMenu(fileName = "GravityDownRule", menuName = "Cellular Automata/Sand Simulation/GravityDownRule")]
public class GravityDownRule : SimulationRule
{
    public CellTypeOld[] DisplaceableTypes;
    public override void ExecuteRule(AutomataController controller, Cell[,] cellGrid, int x, int y, CellTypeOld cellType)
    {
        if (cellGrid[x, y].Next.HasValue)
        {
            return;
        }
        if (!controller.IsIndexValid(x, y - 1))
        {
            return;
        }
        CellTypeOld belowType = cellGrid[x, y - 1].Current;
        CellTypeOld? belowNextType = cellGrid[x, y - 1].Next;

        if (Contains(DisplaceableTypes, belowType) || belowNextType.HasValue && Contains(DisplaceableTypes, belowNextType.Value))
        {
            // Gravity
            cellGrid[x, y].Next = belowNextType.HasValue ? belowNextType.Value : belowType;
            cellGrid[x, y - 1].Next = cellType;
        }
    }
}
