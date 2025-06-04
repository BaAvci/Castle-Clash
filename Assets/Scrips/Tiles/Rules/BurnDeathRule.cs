using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "BurnDeath", menuName = "Cellular Automata/Interaction Rules/Burn Death")]
public class BurnDeathRule : ElementalInteractionRules
{
    public float Timer;

    public override void ExecuteRule(TileManager controller, Tile[,] cellGrid, int x, int y, ElementalEffect elementalEffect)
    {
        controller.StartCoroutine(Co_BurnTimer());
        cellGrid[x, y].Next = elementalEffect;
    }

    private IEnumerator Co_BurnTimer()
    {
        yield return new WaitForSeconds(Timer);
    }
}
