using UnityEngine;

[CreateAssetMenu(fileName = "SingleDirectionSpread", menuName = "Cellular Automata/Interaction Rules/Single Direction Spread")]
public class SingleDirectionSpread : ElementalInteractionRules
{
    public Vector2Int direction;
    public override void ExecuteRule(TileManager controller, Tile[,] cellGrid, int x, int y, ElementalEffect elementalEffect)
    {

    }
}
