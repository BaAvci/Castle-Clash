using UnityEngine;

public abstract class Rule : ScriptableObject
{
    public abstract void ExecuteRule(TileManager controller, Tile[,] cellGrid, int x, int y, ElementalEffect cellType);
}
