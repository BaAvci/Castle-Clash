using System.Linq;
using UnityEngine;

public abstract class SimulationRule : RuleCopy
{
    protected bool Contains(CellTypeOld[] cellTypes, CellTypeOld cellType)
    {
        for (int i = 0; i < cellTypes.Length; i++)
        {
            if (cellTypes[i] == cellType)
            {
                return true;
            }
        }
        return false;
    }
}
