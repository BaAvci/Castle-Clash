using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "OmniDirectionlaSpread", menuName = "Cellular Automata/Interaction Rules/DieOutRule")]
public class DieOut : ElementalInteractionRules
{

    public override void ExecuteRule(TileManager controller, Tile[,] cellGrid, int x, int y, TileType elementalEffect)
    {
        var livingNeighbors = GetNeighborTilesWithEffects(controller, cellGrid, x, y);
        // TODO: Single tile checks if any neighboring tiles have effects that could be applied to it
        // with a check where coutner ellements are present.
        // EX: Target Tile is Grass, on one side is fire on the other water.
        // grass is countered by fire, fire is countered by water. Result should be the normal value.
    }
}
