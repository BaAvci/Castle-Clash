using UnityEngine;

[CreateAssetMenu(fileName = "SlideRule", menuName = "Cellular Automata/Sand Simulation/Slide Rule")]
public class SlideRule : SimulationRule
{
    public CellType[] DisplaceableTypes;

    public override void ExecuteRule(AutomataController controller, Cell[,] cellGrid, int x, int y, CellType cellType)
    {
        if (cellGrid[x, y].Next.HasValue)
        {
            return;
        }

        bool leftValid = controller.IsIndexValid(x - 1, y);
        bool rightValid = controller.IsIndexValid(x + 1, y);

        Cell leftCell = leftValid ? cellGrid[x - 1, y] : null;
        Cell rightCell = rightValid ? cellGrid[x + 1, y] : null;

        CellType? left = leftValid ? leftCell.Next : null;
        CellType? right = rightValid ? rightCell.Next : null;

        left ??= leftValid ? leftCell.Current : null;
        right ??= rightValid ? rightCell.Current : null;

        bool leftAvailable = left.HasValue && Contains(DisplaceableTypes, left.Value);
        bool rightAvailable = right.HasValue && Contains(DisplaceableTypes, right.Value);

        CellType replacementType = CellType.Air;

        if (leftAvailable && rightAvailable)
        {
            if (Random.Range(0f, 1f) < 0.5f)
            {
                replacementType = leftCell.Next.HasValue ? leftCell.Next.Value : leftCell.Current;
                leftCell.Next = cellType;
            }
            else
            {
                replacementType = rightCell.Next.HasValue ? rightCell.Next.Value : rightCell.Current;
                rightCell.Next = cellType;
            }
        }
        else if (leftAvailable)
        {
            replacementType = leftCell.Next.HasValue ? leftCell.Next.Value : leftCell.Current;
            leftCell.Next = cellType;
        }
        else if (rightAvailable)
        {
            replacementType = rightCell.Next.HasValue ? rightCell.Next.Value : rightCell.Current;
            rightCell.Next = cellType;
        }

        if (leftAvailable || rightAvailable)
        {
            cellGrid[x, y].Next = replacementType;
        }
    }
}
