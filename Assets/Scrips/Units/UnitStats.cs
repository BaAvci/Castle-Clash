using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem;
[System.Serializable]
public class UnitStats
{
    // Damage is from main attribute.
    public float Damage;
    public int AttackRange;
    public AttackType AttackType;
    public MainStat MainStat;
    public float AttacksPerSecond;
    public float CurrentDamage { get; private set; }
    #region Strenght
    public float Strenght;                 // Dictates how high the base value the following stats are for a unit
    public float MaxHealthPoints;          // The max amount of HP a unit can have
    public float HealthPointRegen;         // How many HP a unit regend per second
    public float CrowdControlResistance;   // Is a percentage value with a max value of 100
    public float HealingAmplification;     // Percentage value of how much additional healing a unit gets
    public float PhysicalResistance;       // How much damage a unit resists versus physical damage

    private float currentStrenght;
    [SerializeField] private float currentHealthPoints;
    private float currentHealthPointRegen;
    private float currentCrowdControlResistance;
    private float currentHealingAmplification;
    private float currentPhysicalResistance;
    #endregion

    #region Agility                         
    public float Agility;                  // Dictates how high the base value the following stats are for a unit
    public float Speed;                    // How many Tiles per seconds a Unit moves
    public float CritChance;               // Is a percentage Value that displays the chance for a critical hit
    public float EvadeChance;              // Is a percentage value with a max value of 100
    public float AttackSpeed;           // Is a percentage value where the unit can resist critical hits
    public float Armor;                    // How much flat damage a unit negates

    private float currentAgility;
    private float currentSpeed;
    private float currentCritChance;
    private float currentEvadeChance;
    private float currentAttackSpeed;
    private float currentArmor;
    #endregion

    #region Intelligence
    public float Intelligence;             // Dictates how high the base value the following stats are for a unit
    public float MaxMana;                  // How much mana a Unit needs to cast it's ability.
    public float ManaRegen;                // How fast a unit gains mana per second
    public float SpellAmplification;       // How much the unit specific spell is amplified.
    public float StatusResistance;         // Is a percentage value with a max value of 100
    public float MagicalResistance;        // How much damage a unit resists versus Magical damage

    private float currentIntelligence;
    private float currentMana;
    private float currentManaRegen;
    private float currentSpellAmplification;
    private float currentStatusResistance;
    private float currentMagicalResistance;
    #endregion

    private Dictionary<StatusEffect, float> statusEffects = new();
    private Dictionary<OverTimeEffect, float> overTimeEffects = new();

    // Used to set the Gameobject inactive after currentHealth reaches 0;
    private Unit owner;

    public UnitStats(float strenght, float agility, float intelligence, MainStat mainStat, AttackType attackType, int attackRange, Unit owner)
    {
        this.owner = owner;
        SetStats(strenght, agility, intelligence, mainStat, attackType, attackRange);
    }
    public UnitStats(float strenght, float agility, float intelligence, MainStat mainStat, AttackType attackType, int attackRange)
    {
        SetStats(strenght, agility, intelligence, mainStat, attackType, attackRange);
    }

    private void SetStats(float strenght, float agility, float intelligence, MainStat mainStat, AttackType attackType, int attackRange)
    {
        MainStat = mainStat;
        AttackType = attackType;
        AttackRange = attackRange;

        float baseMaxValues = 120;
        float baseResistance = 10;
        float baseChance = 10;
        float baseRegen = 3;
        float baseValue = 10;
        Strenght = strenght;
        MaxHealthPoints = strenght * 22 + baseMaxValues;
        HealthPointRegen = strenght * 0.9f + baseRegen;
        CrowdControlResistance = strenght * 0.05f;
        HealingAmplification = strenght * 0.05f;
        PhysicalResistance = strenght * 0.5f + baseResistance;

        currentStrenght = Strenght;
        currentHealthPoints = MaxHealthPoints;
        currentHealthPointRegen = HealthPointRegen;
        currentCrowdControlResistance = CrowdControlResistance;
        currentHealingAmplification = HealingAmplification;
        currentPhysicalResistance = PhysicalResistance;

        Agility = agility;
        Speed = (agility * 0.5f + baseValue) * 0.1f;
        CritChance = agility * 0.3f;
        EvadeChance = agility * 0.1f;
        float baseAttackSpeed = 1;
        AttackSpeed = baseAttackSpeed - (((int)(agility / 6)) * 0.1f); // TODO: calculte mid range of max agility and set that as 1/atks while max agi is 2/atks and min agi is 0.5/atks || spell modifiers can still chang it to 0.1/atks or 100/atks
        AttacksPerSecond = 1 / AttackSpeed;
        Armor = agility * 0.5f;

        currentAgility = Agility;
        currentSpeed = Speed;
        currentCritChance = CritChance;
        currentEvadeChance = EvadeChance;
        currentAttackSpeed = AttackSpeed;
        currentArmor = Armor;

        Intelligence = intelligence;
        MaxMana = intelligence * 22 + baseMaxValues;
        ManaRegen = intelligence * 1.2f + baseRegen;
        SpellAmplification = intelligence * 0.2f;
        StatusResistance = intelligence * 0.5f;
        MagicalResistance = intelligence * 0.5f + baseResistance;

        currentIntelligence = intelligence;
        currentMana = MaxMana;
        currentManaRegen = ManaRegen;
        currentSpellAmplification = SpellAmplification;
        currentStatusResistance = StatusResistance;
        currentMagicalResistance = MagicalResistance;

        float baseDamage = 20;
        Damage += baseDamage;
        switch (mainStat)
        {
            case MainStat.Strenght:
                Damage += strenght;
                CrowdControlResistance += baseResistance;
                break;
            case MainStat.Agility:
                Damage += agility;
                CritChance += agility * 1.5f + baseChance;
                EvadeChance += agility * 1.5f + baseChance;
                break;
            case MainStat.Intelligence:
                Damage += intelligence;
                SpellAmplification += intelligence * 1.5f + baseMaxValues;
                StatusResistance += intelligence * 0.5f + baseResistance;
                break;
        }
        if (attackType == AttackType.Ranged)
        {
            Damage *= 0.8f;
        }
        CurrentDamage = Damage;
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

#if UNITY_EDITOR
        Debug.Log($"Modified changevalue is {changeValue}.");
        Debug.Log($"Modified currentStatValue is {currentStatValue}.");
#endif
        if (changeValue + currentStatValue >= maxStatValue)
        {
            return maxStatValue - currentStatValue;
        }
        return changeValue;
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
                currentArmor += ModifyStat(instance, currentArmor, Armor, selectedStat);
                break;
            case AffectedStat.Damage:
                CurrentDamage += ModifyStat(instance, CurrentDamage, Damage, selectedStat);
                break;
            case AffectedStat.HealthPoints:
                var valueChange = ModifyStat(instance, currentHealthPoints, MaxHealthPoints, AffectedStat.HealthPoints);
                if (instance.InstantEffects.IsPositiv)
                {
                    valueChange += valueChange * (HealingAmplification / 100);
                }
                currentHealthPoints += valueChange;
                break;
            case AffectedStat.Mana:
                currentMana += ModifyStat(instance, currentMana, MaxMana, selectedStat);
                break;
            case AffectedStat.HealthRegen:
                currentHealthPointRegen += ModifyStat(instance, currentHealthPointRegen, HealthPointRegen, selectedStat);
                break;
            case AffectedStat.ManaRegen:
                currentManaRegen += ModifyStat(instance, currentManaRegen, ManaRegen, selectedStat);
                break;
            case AffectedStat.Agility:
                currentAgility += ModifyStat(instance, currentAgility, Agility, selectedStat);
                break;
            case AffectedStat.Intelligence:
                currentIntelligence += ModifyStat(instance, currentIntelligence, Intelligence, selectedStat);
                break;
            case AffectedStat.Strenght:
                currentStrenght += ModifyStat(instance, currentStrenght, Strenght, selectedStat);
                break;
            case AffectedStat.CrowdControlResistance:
                currentCrowdControlResistance += ModifyStat(instance, currentCrowdControlResistance, CrowdControlResistance, selectedStat);
                break;
            case AffectedStat.PhysicalResistance:
                currentPhysicalResistance += ModifyStat(instance, currentPhysicalResistance, PhysicalResistance, selectedStat);
                break;
            case AffectedStat.HealingAmplification:
                currentHealingAmplification += ModifyStat(instance, currentHealingAmplification, HealingAmplification, selectedStat);
                break;
            case AffectedStat.Speed:
                currentSpeed += ModifyStat(instance, currentSpeed, Speed, selectedStat);
                break;
            case AffectedStat.CritChance:
                currentCritChance += ModifyStat(instance, currentCritChance, CritChance, selectedStat);
                break;
            case AffectedStat.EvadeChance:
                currentEvadeChance += ModifyStat(instance, currentEvadeChance, EvadeChance, selectedStat);
                break;
            case AffectedStat.CritResistance:
                currentCritChance += ModifyStat(instance, currentAttackSpeed, AttackSpeed, selectedStat);
                break;
            case AffectedStat.StatusResistance:
                currentStatusResistance += ModifyStat(instance, currentStatusResistance, StatusResistance, selectedStat);
                break;
            case AffectedStat.MagicalResistance:
                currentMagicalResistance += ModifyStat(instance, currentMagicalResistance, MagicalResistance, selectedStat);
                break;
            case AffectedStat.SpellAmplification:
                currentSpellAmplification += ModifyStat(instance, currentSpellAmplification, SpellAmplification, selectedStat);
                //Special cases following
                break;
            case AffectedStat.Regen:
                CurrentDamage += ModifyStat(instance, currentAgility, Damage, selectedStat);
                break;
            case AffectedStat.BaseStats:
                break;
            case AffectedStat.AllStats:
                CurrentDamage += ModifyStat(instance, CurrentDamage, Damage, selectedStat);
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
            UnitMonoMule.Instance.StartCoroutine(Co_ApplyEffect(statusEffect));
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
            UnitMonoMule.Instance.StartCoroutine(Co_OverTimeEffectApplication(overTimeEffect));
        }
    }

    public bool RecieveDamage(float incomingDamage)
    {
        float calculatedPhysicalResistance = incomingDamage * (currentPhysicalResistance * 0.01f);
        float resultingDamage = Mathf.Abs(incomingDamage - calculatedPhysicalResistance - currentArmor);
        currentHealthPoints -= resultingDamage;
        if (currentHealthPoints <= 0)
        {
            Animator animator = owner.transform.root.GetComponent<Animator>();
            animator.SetTrigger("Death");
            owner.enabled = false;
            return true;
        }
        return false;
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
