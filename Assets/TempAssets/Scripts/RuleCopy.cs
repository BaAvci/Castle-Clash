using UnityEngine;

public abstract class RuleCopy : ScriptableObject
{
    public abstract void ExecuteRule(AutomataController controller, Cell[,] cellGrid, int x, int y, CellTypeOld cellType);
}
