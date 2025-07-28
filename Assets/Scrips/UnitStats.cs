using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;

public class UnitStats : MonoBehaviour
{
    // Damage is from main attribute.
    private float damage;
    private float currentDamage;
    #region Strenght
    private float strenght;                 // Dictates how high the base value the following stats are for a unit
    private float maxHealthPoints;          // The max amount of HP a unit can have
    private float healthPointRegen;         // How many HP a unit regend per second
    private float crowdControlResistance;   // Is a percentage value with a max value of 100
    private float healingAmplification;     // Percentage value of how much additional healing a unit gets
    private float physicalResistance;       // How much damage a unit resists versus physical damage

    private float currentStrenght;
    private float currentHealthPoints;
    private float currentHealthPointRegen;
    private float currentCrowdControlResistance;
    private float currentHealingAmplification;
    private float currentPhysicalResistance;
    #endregion

    #region Agility                         
    private float agility;                  // Dictates how high the base value the following stats are for a unit
    private float speed;                    // How many Tiles per seconds a Unit moves
    private float critChance;               // Is a percentage Value that displays the chance for a critical hit
    private float evadeChance;              // Is a percentage value with a max value of 100
    private float critResistance;           // Is a percentage value where the unit can resist critical hits
    private float armor;                    // How much flat damage a unit negates

    private float currentAgility;
    private float currentSpeed;
    private float currentCritChance;
    private float currentEvadeChance;
    private float currentCritResistance;
    private float currentArmor;
    #endregion

    #region Intelligence
    private float intelligence;             // Dictates how high the base value the following stats are for a unit
    private float maxMana;                  // How much mana a Unit needs to cast it's ability.
    private float manaRegen;                // How fast a unit gains mana per second
    private float spellAmplification;       // How much the unit specific spell is amplified.
    private float statusResistance;         // Is a percentage value with a max value of 100
    private float magicalResistance;        // How much damage a unit resists versus Magical damage

    private float currentIntelligence;
    private float currentMana;
    private float currentManaRegen;
    private float currentSpellAmplification;
    private float currentStatusResistance;
    private float currentMagicalResistance;
    #endregion

    private Dictionary<StatusEffect, float> statusEffects = new();
    private Dictionary<OverTimeEffect, float> overTimeEffects = new();

    private void Start()
    {
        maxHealthPoints = 200;
        currentHealthPoints = maxHealthPoints;
    }

    private float ModifyStat(InstantEffect effect, float currentStatValue, float maxStatValue, AffectedStat affectedStat)
    {

#if UNITY_EDITOR
        Debug.Log($"Instance Effect {effect.InstantEffects.name}");
        Debug.Log($"currently selected stat: {affectedStat}.");
        Debug.Log($"Values are: Current {currentStatValue} and Max value {maxStatValue}");
#endif

        float changeValue;
        if (effect.InstantEffects.IsCurrentPercentage)
        {
            changeValue = currentStatValue / 100 * effect.Value;
        }
        else if (effect.InstantEffects.IsMaxPercentage)
        {
            changeValue = maxStatValue / 100 * effect.Value;
        }
        else
        {
            changeValue = effect.Value;
        }

        if (!effect.InstantEffects.IsPositiv)
        {
            changeValue *= -1;
        }

#if UNITY_EDITOR
        Debug.Log($"Current changevalue is {changeValue}.");
#endif

        changeValue = CalculateChangeValueWithStatusEffects(changeValue, affectedStat, effect.InstantEffects.IsPositiv);
        currentStatValue += changeValue;

#if UNITY_EDITOR
        Debug.Log($"Modified changevalue is {changeValue}.");
        Debug.Log($"Modified currentStatValue is {currentStatValue}.");
#endif
        return currentStatValue;
    }

    public void ApplyInstanceModification(InstantEffect instance)
    {
#if UNITY_EDITOR
        if (instance.InstantEffects.IsCurrentPercentage && instance.InstantEffects.IsMaxPercentage)
        {
            Debug.LogError("Please only enter one percentage value!");
        }
        Debug.Log(instance.GetDiscription());
#endif
        AffectedStat selectedStat = instance.InstantEffects.AffectedStat;
        switch (selectedStat)
        {
            case AffectedStat.Armor:
                currentArmor = ModifyStat(instance, currentArmor, armor, selectedStat);
                break;
            case AffectedStat.Damage:
                currentDamage = ModifyStat(instance, currentDamage, damage, selectedStat);
                break;
            case AffectedStat.HealthPoints:
                var valueChange = ModifyStat(instance, currentHealthPoints, maxHealthPoints, AffectedStat.HealthPoints);
                if (instance.InstantEffects.IsPositiv)
                {
                    valueChange += valueChange * (healingAmplification / 100);
                }
                currentHealthPoints += valueChange;
                break;
            case AffectedStat.Mana:
                currentMana = ModifyStat(instance, currentMana, maxMana, selectedStat);
                break;
            case AffectedStat.HealthRegen:
                currentHealthPointRegen = ModifyStat(instance, currentHealthPointRegen, healthPointRegen, selectedStat);
                break;
            case AffectedStat.ManaRegen:
                currentManaRegen = ModifyStat(instance, currentManaRegen, manaRegen, selectedStat);
                break;
            case AffectedStat.Agility:
                currentAgility = ModifyStat(instance, currentAgility, agility, selectedStat);
                break;
            case AffectedStat.Intelligence:
                currentIntelligence = ModifyStat(instance, currentIntelligence, intelligence, selectedStat);
                break;
            case AffectedStat.Strenght:
                currentStrenght = ModifyStat(instance, currentStrenght, strenght, selectedStat);
                break;
            case AffectedStat.CrowdControlResistance:
                currentCrowdControlResistance = ModifyStat(instance, currentCrowdControlResistance, crowdControlResistance, selectedStat);
                break;
            case AffectedStat.PhysicalResistance:
                currentPhysicalResistance = ModifyStat(instance, currentPhysicalResistance, physicalResistance, selectedStat);
                break;
            case AffectedStat.HealingAmplification:
                currentHealingAmplification = ModifyStat(instance, currentHealingAmplification, healingAmplification, selectedStat);
                break;
            case AffectedStat.Speed:
                currentSpeed = ModifyStat(instance, currentSpeed, speed, selectedStat);
                break;
            case AffectedStat.CritChance:
                currentCritChance = ModifyStat(instance, currentCritChance, critChance, selectedStat);
                break;
            case AffectedStat.EvadeChance:
                currentEvadeChance = ModifyStat(instance, currentEvadeChance, evadeChance, selectedStat);
                break;
            case AffectedStat.CritResistance:
                currentCritChance = ModifyStat(instance, currentCritResistance, critResistance, selectedStat);
                break;
            case AffectedStat.StatusResistance:
                currentStatusResistance = ModifyStat(instance, currentStatusResistance, statusResistance, selectedStat);
                break;
            case AffectedStat.MagicalResistance:
                currentMagicalResistance = ModifyStat(instance, currentMagicalResistance, magicalResistance, selectedStat);
                break;
            case AffectedStat.SpellAmplification:
                currentSpellAmplification = ModifyStat(instance, currentSpellAmplification, spellAmplification, selectedStat);
                //Special cases following
                break;
            case AffectedStat.Regen:
                currentDamage = ModifyStat(instance, currentAgility, damage, selectedStat);
                break;
            case AffectedStat.BaseStats:
                break;
            case AffectedStat.AllStats:
                currentDamage = ModifyStat(instance, currentDamage, damage, selectedStat);
                break;
            case AffectedStat.Resistance:
                break;
            case AffectedStat.Amplification:
                break;
            default:
                break;
        }
    }

    public void ApplyStatusEffect(StatusEffect statusEffect)
    {
        if (statusEffects.ContainsKey(statusEffect))
        {
            foreach (var effect in statusEffects.Keys)
            {
                if (effect.Equals(statusEffect))
                {
                    effect.UpdateStatusDuration(statusEffect.DefaultDuration);
                }
            }
        }
        else
        {
            StartCoroutine(Co_ApplyEffect(statusEffect));
        }
    }

    public void ApplyOverTimeEffect(OverTimeEffect overTimeEffect)
    {
        if (overTimeEffects.ContainsKey(overTimeEffect))
        {
            foreach (var effect in overTimeEffects.Keys)
            {
                if (effect.Equals(overTimeEffect))
                {
                    effect.UpdateValuesOnAffectedUnit(overTimeEffect.InstanceEffect.DefaultValue, overTimeEffect.Duration);
                }
            }
        }
        else
        {
            StartCoroutine(Co_OverTimeEffectApplication(overTimeEffect));
        }
    }

    private IEnumerator Co_ApplyEffect(StatusEffect statusEffect)
    {
        statusEffects.Add(statusEffect, 0);
        while (statusEffects[statusEffect] <= statusEffect.Duration)
        {
            Debug.Log($"{statusEffect.SOStatusEffect.name} has run for {statusEffects[statusEffect]} seconds");
            yield return new WaitForSeconds(1);
            statusEffects[statusEffect] += 1;
        }
        statusEffect.ResetValues();
        statusEffects.Remove(statusEffect);
    }

    private IEnumerator Co_OverTimeEffectApplication(OverTimeEffect overTimeEffect)
    {
        overTimeEffects.Add(overTimeEffect, 0);
        while (overTimeEffects[overTimeEffect] <= overTimeEffect.Duration)
        {
            Debug.Log($"{overTimeEffect.GetDiscription()} has run for {overTimeEffects[overTimeEffect]} seconds");
            overTimeEffect.InstanceEffect.ApplyEffect(this);
            yield return new WaitForSeconds(1);
            overTimeEffects[overTimeEffect] += 1;
        }
        overTimeEffect.ResetValues();
        overTimeEffects.Remove(overTimeEffect);
    }
    private int CalculateChangeValueWithStatusEffects(float changeValue, AffectedStat affectedStat, bool isPositiv)
    {
        float percentageValues = 0;
        float flatValues = 0;
        foreach (var statusEffect in statusEffects)
        {
            SOStatusEffect sOStatusEffect = statusEffect.Key.SOStatusEffect;
            if (sOStatusEffect.AffectedStat == affectedStat && sOStatusEffect.IsPositiv == isPositiv)
            {
                if (sOStatusEffect.IsPercentage)
                {
                    percentageValues += statusEffect.Key.Value;
                }
                else
                {
                    flatValues += statusEffect.Key.Value;
                }
            }
        }
        changeValue += changeValue * (percentageValues / 100);
        changeValue += flatValues;
        return (int)changeValue;
    }
}
