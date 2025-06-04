using UnityEngine;

[CreateAssetMenu(fileName = "BurnSpread", menuName = "Cellular Automata/Interaction Rules/Burn Spread")]
public class BurnSpreadingRule : ElementalInteractionRules
{
    /// <summary>
    /// Effects that can burn this Object
    /// </summary>
    [SerializeField] private ElementalEffect[] DangerousEffects;

    /// <summary>
    /// Effects that can hinder the DangerousEffects from burning this
    /// </summary>
    [SerializeField] private ElementalEffect[] SaveEffects;

    /// <summary>
    /// Amount of DangerousEffects have to affect the tile for it to affect
    /// </summary>
    [SerializeField] private float PointsTillStatusChange;
    //public ElementalEffect EffectIfFulfilled;

    public override void ExecuteRule(TileManager controller, Tile[,] cellGrid, int x, int y, ElementalEffect elementalEffect)
    {
        float dangerValue = CountValuesOfNeighbors(DangerousEffects, controller, cellGrid, x, y, true);
        float saveValue = cellGrid[x, y].Current.SaveMultiplier;

        if (PointsTillStatusChange > dangerValue * saveValue)
        {
            PointsTillStatusChange -= dangerValue * saveValue;
            cellGrid[x, y].Next = elementalEffect;
        }
    }
}
