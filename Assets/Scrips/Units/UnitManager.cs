using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Build.Pipeline.Injector;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public event Action<Unit, bool> UnitAmountOnBoardChanged;
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
    public void Initialize(Actor player, Actor Enemy)
    {
        this.player = player;
        this.enemy = Enemy;
        unitsOnBoard.Clear();
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

    public Dictionary<Actor, List<Unit>> GetAllUnitsForActors()
    {
        Dictionary<Actor, List<Unit>> units = new();
        List<Unit> playerUnits = new List<Unit>();
        List<Unit> enemyUnits = new List<Unit>();
        foreach (Unit unit in unitsOnBoard.Keys)
        {
            if (unit.PlayerOwned)
            {
                playerUnits.Add(unit);
            }
            else
            {
                enemyUnits.Add(unit);
            }
        }
        units.Add(player, playerUnits);
        units.Add(enemy, enemyUnits);
        return units;
    }

    private void InitializeUnit(Vector3 position, GameObject spawnedUnit)
    {
        Unit newUnit = spawnedUnit.transform.root.GetComponentInChildren<Unit>(true);
        Vector2Int fixedPos = new Vector2Int((int)position.x, (int)position.z);
        newUnit.GetComponentInChildren<UnitMovement>().UnitMovedTile += UpdateUnitTargets;
        newUnit.IsDead += RemoveUnitFromDictionary;
        unitsOnBoard.Add(newUnit, fixedPos);
        UnitAmountOnBoardChanged?.Invoke(newUnit, true);
        UpdateUnitTargets(newUnit, position);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="unit"></param>
    /// <param name="pos"></param>
    /// <returns>Returns true if the Unit has to send position updates more often</returns>
    private bool UpdateUnitTargets(Unit unit, Vector3 pos)
    {
        Unit closestTarget = null;
        float closestTargetPos = float.MaxValue;
        Vector2Int cleanedPosition = new Vector2Int((int)pos.x, (int)pos.z);

        bool unitReachedEnd = UpdateUnitPosition(unit, pos, cleanedPosition);
        if (unitReachedEnd)
        {
            return false;
        }

        foreach (var possibleTarget in unitsOnBoard.ToList())
        {
            if (possibleTarget.Value.y != pos.z || possibleTarget.Key.PlayerOwned == unit.PlayerOwned)
            {
                continue;
            }
            if ((unit.PlayerOwned && possibleTarget.Key.gameObject.transform.position.x <= unit.gameObject.transform.position.x) || (!unit.PlayerOwned && possibleTarget.Key.gameObject.transform.position.x >= unit.gameObject.transform.position.x))
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
        unit.GetComponentInChildren<UnitAttack>().SetTarget(closestTarget);
        return false;
    }

    private bool UpdateUnitPosition(Unit unit, Vector3 pos, Vector2Int cleanedPosition)
    {
        if (cleanedPosition.x != unitsOnBoard[unit].x)
        {
            unitsOnBoard[unit] = cleanedPosition;
        }

        Actor target = unit.PlayerOwned ? enemy : player;
        // has the unit reached the enemy end?
        bool reachedEnde = unit.PlayerOwned
            ? pos.x >= target.GridStartPos.x
            : pos.x <= target.GridStartPos.x;

        if (reachedEnde)
        {
            Actor damageTarget = unit.PlayerOwned ? enemy : player;
            damageTarget.ReceiveDamage();
            unit.gameObject.SetActive(false);
            return true;
        }

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
        UnitAmountOnBoardChanged?.Invoke(unitToRemove, false);
    }
}
