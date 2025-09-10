using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class AIBoardKnowledge
{
    public float AiHealth { get; private set; }
    public float PlayerHealth { get; private set; }
    public List<Unit> AiUnits { get; private set; }
    public List<Unit> PlayerUnits { get; private set; }
    public int Energy { get; private set; }
    public Actor Owner { get; private set; }
    private Vector2Int gridSize;

    public AIBoardKnowledge(UnitManager unitManager, Actor ai, Actor player)
    {
        gridSize = new Vector2Int(ai.GridStartPos.x, ai.GridMaxSpellPlayPos.y);
        AiUnits = new();
        PlayerUnits = new();
        Owner = ai;

        AiHealth = ai.HealthPoints;
        PlayerHealth = player.HealthPoints;

        Owner.HealthChanged += UpdateHP;
        player.HealthChanged += UpdateHP;
        unitManager.UnitAmountOnBoardChanged += UpdateUnits;
    }

    private void UpdateHP(Actor actor, float newHp)
    {
        if (actor.IsPlayer)
        {
            PlayerHealth = newHp;
        }
        else
        {
            AiHealth = newHp;
        }
    }
    private void UpdateUnits(Unit unit, bool unitAdded)
    {
        List<Unit> selectedActor = unit.PlayerOwned ? PlayerUnits : AiUnits;
        if (unitAdded)
        {
            selectedActor.Add(unit);
        }
        else
        {
            selectedActor.Remove(unit);
        }
    }
    public void UpdateEnergy(int newEnergyValue)
    {

    }
    public List<UnitTimeToKill> CalculateUnitTimeTillEndOfBoard()
    {
        List<UnitTimeToKill> units = new List<UnitTimeToKill>();
        foreach (Unit unit in PlayerUnits)
        {
            float distance = gridSize.x - unit.gameObject.transform.position.x;
            int correctedDistance = Mathf.RoundToInt(distance);
            UnitTimeToKill newUnit = new(unit, correctedDistance);
            units.Add(newUnit);
        }
        return units;
    }
}
