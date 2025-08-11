using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline.Injector;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    private Dictionary<Unit, Vector2Int> unitsOnBoard = new();
    private Actor player;
    private Actor enemy;

    public void Initialize(Actor player, Actor Enemy, Dictionary<Unit, Vector2Int> existingUnits)
    {
        this.player = player;
        this.enemy = Enemy;
        unitsOnBoard.Clear();
        foreach (var unit in existingUnits)
        {
            unitsOnBoard.Add(unit.Key, unit.Value);
        }
    }

    public List<Unit> GetAllUnitsInRange(Vector2 targetPosition, int range = 1)
    {
        List<Unit> unitsInRange = new();
        Vector2Int normedPosition = new Vector2Int((int)targetPosition.x, (int)targetPosition.y);
        for (int x = -range; x < range + 1; x++)
        {
            for (int y = -range; y < range + 1; y++)
            {
                if (Mathf.Abs(x) + Mathf.Abs(y) > range)
                {
                    continue;
                }
                Vector2Int positionToCheck = new Vector2Int(normedPosition.x + x, normedPosition.y + y);

                foreach (var unit in unitsOnBoard)
                {
                    if (unit.Value == positionToCheck)
                    {
                        unitsInRange.Add(unit.Key);
                    }
                }
            }
        }
        return unitsInRange;
    }

    public void RegisterCardEvent(PlayableCard playableCard)
    {
        playableCard.CardWithGameObjectSpawned += InitializeUnit;
    }
    public void UnRegisterCardEvent(PlayableCard playableCard)
    {
        playableCard.CardWithGameObjectSpawned -= InitializeUnit;
    }

    private void InitializeUnit(Vector3 position, GameObject spawnedUnit)
    {
        Unit newUnit = spawnedUnit.GetComponent<Unit>();
        Vector2Int fixedPos = new Vector2Int((int)position.x, (int)position.z);
        newUnit.GetComponent<UnitMovement>().UnitMovedTile += UpdateUnitTargets;
        newUnit.IsDead += RemoveUnitFromDictionary;
        unitsOnBoard.Add(newUnit, fixedPos);
        UpdateUnitTargets(newUnit, position);
    }

    private bool UpdateUnitTargets(Unit unit, Vector3 pos)
    {
        Unit closestTarget = null;
        float closestTargetPos = float.MaxValue;
        Vector2Int cleanedPosition = new Vector2Int((int)pos.x, (int)pos.z);

        if (cleanedPosition.x != unitsOnBoard[unit].x)
        {
            unitsOnBoard[unit] = cleanedPosition;
        }

        foreach (var possibleTarget in unitsOnBoard.ToList())
        {
            if (possibleTarget.Value.y != pos.z || possibleTarget.Key.PlayerOwned == unit.PlayerOwned)
            {
                continue;
            }

            // Is Unit in range
            if (CalculateAttackRange(unit, possibleTarget.Key))
            {
                float distance = Mathf.Abs(possibleTarget.Value.x - pos.x);
                if (distance < closestTargetPos)
                {
                    closestTargetPos = distance;
                    closestTarget = possibleTarget.Key;
                }
            }
        }
        unit.GetComponent<UnitAttack>().SetTarget(closestTarget);
        return false;
    }

    private bool CalculateAttackRange(Unit attacker, Unit toBeCheckedObject)
    {
        var distanceToObject = (toBeCheckedObject.transform.position - attacker.transform.position).sqrMagnitude;
        if (distanceToObject > attacker.UnitStats.AttackRange * attacker.UnitStats.AttackRange)
        {
            return false;
        }
        return true;
    }

    private void RemoveUnitFromDictionary(Unit unitToRemove)
    {
        unitsOnBoard.Remove(unitToRemove);
        unitToRemove.IsDead -= RemoveUnitFromDictionary;
        unitToRemove.GetComponent<UnitMovement>().UnitMovedTile -= UpdateUnitTargets;
    }
}
