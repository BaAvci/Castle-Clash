using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "MovingApplication", menuName = "Cellular Automata/Interaction Rules/Moving Application")]
public class MovingApplicationRule : ElementalInteractionRules
{
    public override void ExecuteRule(TileManager controller, Tile[,] cellGrid, int x, int y, ElementalEffect elementalEffect)
    {
        cellGrid[x, y].Next = elementalEffect;
    }
}
