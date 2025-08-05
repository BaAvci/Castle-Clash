using System.Collections.Generic;
using System.Linq;
using UnityEditor.Build.Pipeline.Injector;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> unitList;
    private void Update()
    {
        for (int i = 0; i < unitList.Count; i++)
        {
            Vector3 unitPos = unitList[i].transform.position;
            List<GameObject> enemyUnitsOnSameLane = unitList.Where(u => u.transform.position.z == unitPos.z && u.transform.position != unitPos && u.activeSelf).ToList();
            Unit closestTarget = null;
            for (int j = 0; enemyUnitsOnSameLane.Count > j; j++)
            {
                Unit original = unitList[i].GetComponent<Unit>();
                Unit possibleTarget = enemyUnitsOnSameLane[j].GetComponent<Unit>();
                if (possibleTarget.PlayerOwned != original.PlayerOwned && CalculateAttackRange(original, possibleTarget))
                {
                    if (closestTarget == null)
                    {
                        closestTarget = possibleTarget;
                    }
                    else
                    {
                        float possiblePos = possibleTarget.transform.position.x;
                        float closestTargetPos = closestTarget.transform.position.x;
                        float possibleDist = possiblePos - unitPos.x;
                        float closestDist = closestTargetPos - unitPos.x;
                        if (closestDist > possibleDist)
                        {
                            closestTarget = possibleTarget;
                        }
                    }
                    original.GetComponent<UnitAttack>().SetTarget(closestTarget);
                }
            }
        }
    }
    private void CheckRanges()
    {
        List<Unit> units = new List<Unit>();

        /*
         * on unit spawn add unit to list
         * check distance of all units
         * if another unit has possible attack range to another unit that is not on the same team
         */
    }

    private bool CalculateAttackRange(Unit attacker, Unit toBeCheckedObject)
    {
        var distanceToObject = (toBeCheckedObject.transform.position - attacker.transform.position).sqrMagnitude;
        if (distanceToObject <= attacker.UnitStats.AttackRange * attacker.UnitStats.AttackRange)
        {
            return true;
        }
        return false;
    }
}
