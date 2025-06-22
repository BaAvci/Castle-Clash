using UnityEngine;

[CreateAssetMenu(fileName = "GravitySlideRule", menuName = "Cellular Automata/Sand Simulation/GravitySlideRule")]
public class GravitySlideRule : SimulationRule
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

        bool belowLeftValid = controller.IsIndexValid(x - 1, y - 1);
        bool belowRightValid = controller.IsIndexValid(x + 1, y - 1);

        CellTypeOld? belowLeft = belowLeftValid ? cellGrid[x - 1, y - 1].Next : null;
        CellTypeOld? belowRight = belowRightValid ? cellGrid[x + 1, y - 1].Next : null;
        belowLeft ??= belowLeftValid ? cellGrid[x - 1, y - 1].Current : null;
        belowRight ??= belowRightValid ? cellGrid[x + 1, y - 1].Current : null;

        bool belowLeftAvailable = belowLeft.HasValue && Contains(DisplaceableTypes, belowLeft.Value);
        bool belowRightAvailable = belowRight.HasValue && Contains(DisplaceableTypes, belowRight.Value);

        CellTypeOld replacementType = CellTypeOld.Air;

        if (belowLeftAvailable && belowRightAvailable)
        {
            if (Random.Range(0f, 1f) < 0.5f)
            {
                replacementType = cellGrid[x - 1, y - 1].Next.HasValue ? cellGrid[x - 1, y - 1].Next.Value : cellGrid[x - 1, y - 1].Current;
                cellGrid[x - 1, y - 1].Next = cellType;
            }
            else
            {
                replacementType = cellGrid[x + 1, y - 1].Next.HasValue ? cellGrid[x + 1, y - 1].Next.Value : cellGrid[x + 1, y - 1].Current;
                cellGrid[x + 1, y - 1].Next = cellType;
            }
        }
        else if (belowLeftAvailable && !belowRightAvailable)
        {
            replacementType = cellGrid[x - 1, y - 1].Next.HasValue ? cellGrid[x - 1, y - 1].Next.Value : cellGrid[x - 1, y - 1].Current;
            cellGrid[x - 1, y - 1].Next = cellType;
        }
        else if (!belowLeftAvailable && belowRightAvailable)
        {
            replacementType = cellGrid[x + 1, y - 1].Next.HasValue ? cellGrid[x + 1, y - 1].Next.Value : cellGrid[x + 1, y - 1].Current;
            cellGrid[x + 1, y - 1].Next = cellType;
        }

        if(belowLeftAvailable || belowRightAvailable)
        {
            cellGrid[x,y].Next = replacementType;
        }
    }
}