using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

public class Planner
{
    public Dictionary<AIAction, Unit> Plan(AIBoardKnowledge boardKnowledge, AIGoal goal, List<AIAction> availableActions)
    {
        Dictionary<AIAction, Unit> selectedAction = new();
        Dictionary<AIAction, List<UnitTimeToKill>> actionToTargets = new();
        foreach (AIAction action in availableActions)
        {
            if (action.CheckPrecondtions(boardKnowledge, goal))
            {
                List<UnitTimeToKill> targets = CalculateCardTargets(action.Card, goal, boardKnowledge);
                actionToTargets.Add(action, targets);
            }
        }

        foreach (AIAction action in actionToTargets.Keys)
        {
            selectedAction.Add(action, null);
            for (int i = 0; i < actionToTargets[action].Count; i++)
            {
                Unit selectedUnit = actionToTargets[action][i].Unit;
                if (selectedAction.ContainsValue(selectedUnit))
                    continue;
                selectedAction[action] = selectedUnit;
            }
        }
        return selectedAction;
    }

    private List<UnitTimeToKill> CalculateCardTargets(PlayableCard card, AIGoal goal, AIBoardKnowledge boardKnowledge)
    {
        List<UnitTimeToKill> unitTimeToKills = boardKnowledge.CalculateUnitTimeTillEndOfBoard();
        unitTimeToKills = CalculateTimeToKill(card, unitTimeToKills, boardKnowledge);
        if (goal is SurviveGoal)
        {
            unitTimeToKills?.Sort((a, b) =>
            {
                int result = a.Distance.CompareTo(b.Distance);
                if (result == 0)
                {
                    result = b.TimeToKill.CompareTo(a.TimeToKill);
                }
                return result;
            });
        }
        else
        {
            unitTimeToKills?.Sort((a, b) =>
            {
                int result = a.Distance.CompareTo(b.Distance);
                if (result == 0)
                {
                    result = a.TimeToKill.CompareTo(b.TimeToKill);
                }
                return result;
            });
        }
        return unitTimeToKills;
    }

    private List<UnitTimeToKill> CalculateTimeToKill(PlayableCard card, List<UnitTimeToKill> unitTimeToKills, AIBoardKnowledge boardKnowledge)
    {
        if (card is PlayableUnitCard)
        {
            Unit selectedCardUnit = card.CardData.Gameobject.GetComponentInChildren<Unit>();
            for (int i = 0; i < unitTimeToKills.Count; i++)
            {
                UnitTimeToKill currentUnit = unitTimeToKills[i];
                // IF AI Card is smaller equal THAN the Unit on the Board THEN Continue
                if (selectedCardUnit.Stats.UnitSpawnAnimationDuration <= currentUnit.Distance)
                    continue;

                float attacksToKill = Mathf.Ceil(currentUnit.Unit.UnitStats.MaxHealthPoints / selectedCardUnit.UnitStats.Damage);
                currentUnit.TimeToKill = attacksToKill * selectedCardUnit.UnitStats.AttackSpeed;

                float attacksToDie = Mathf.Ceil(selectedCardUnit.UnitStats.MaxHealthPoints / currentUnit.Unit.UnitStats.Damage);
                float timeTillDeath = attacksToDie * currentUnit.Unit.UnitStats.AttackSpeed;

                currentUnit.IsDeadAfterwards = currentUnit.TimeToKill < timeTillDeath;

                unitTimeToKills[i] = currentUnit;
            }
        }
        else if (card is PlayableSpellCard)
        {
            for (int i = 0; i < unitTimeToKills.Count; i++)
            {
                UnitTimeToKill currentUnit = unitTimeToKills[i];

                if (boardKnowledge.Owner.GridMaxSpellPlayPos.x < currentUnit.Distance)
                    continue;

                // TODO: should check if another effect was already applied to this unit. Incase if something has an initial damage value and then an overtime effect that would kill that unit.
                foreach (IEffect effect in card.Effects)
                {
                    if (effect is OverTimeEffect overTimeEffect && !overTimeEffect.InstanceEffect.InstantEffects.IsPositiv)
                    {
                        currentUnit.TimeToKill = Mathf.Ceil(currentUnit.Unit.UnitStats.MaxHealthPoints / overTimeEffect.InstanceEffect.Value);
                        currentUnit.IsDeadAfterwards = currentUnit.TimeToKill <= overTimeEffect.Duration;
                    }
                    else if (effect is InstantEffect instantEffect && !instantEffect.InstantEffects.IsPositiv)
                    {
                        currentUnit.IsDeadAfterwards = currentUnit.Unit.UnitStats.MaxHealthPoints <= instantEffect.Value;
                        currentUnit.IsDeadAfterwards = currentUnit.Unit.UnitStats.MaxHealthPoints <= instantEffect.Value;
                        if (currentUnit.IsDeadAfterwards)
                        {
                            currentUnit.TimeToKill = 0;
                        }
                        else
                        {
                            currentUnit.TimeToKill = float.MaxValue;
                        }
                    }
                }
                unitTimeToKills[i] = currentUnit;
            }
        }
        return unitTimeToKills;
    }
}

public struct UnitTimeToKill
{
    public int Distance;
    public Unit Unit;
    public float TimeToKill;
    public bool IsDeadAfterwards;

    public UnitTimeToKill(Unit unit, int distance) : this()
    {
        Unit = unit;
        Distance = distance;
    }
}