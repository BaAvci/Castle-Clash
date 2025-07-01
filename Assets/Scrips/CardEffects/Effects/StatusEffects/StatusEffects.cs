using UnityEngine;

public enum AffectedStat
{
    /// <summary>
    /// How much damage is blocked
    /// </summary>
    Armor = 1,
    /// <summary>
    /// How much damage something deals to something else
    /// </summary>
    Damage = 2,
    /// <summary>
    /// How much HP a unit has
    /// </summary>
    HealthPoints = 3,
    /// <summary>
    /// How much Mana a unit has
    /// </summary>
    Mana = 4,
    /// <summary>
    /// How much HP a unit is regening per second
    /// </summary>
    HealthRegen = 5,
    /// <summary>
    /// How much Mana a unit is regening per second
    /// </summary>
    ManaRegen = 6,
    /// <summary>
    /// Affects all kinds of regeneration
    /// </summary>
    Regen = 7,
    /// <summary>
    /// Affects the Damage of tank units, the HP and HP regen of any unit
    /// </summary>
    Strenght = 7,
    /// <summary>
    /// Affects the Damage of swift units, the Armor and Movement/Attack speed of any unit
    /// </summary>
    Agility = 8,
    /// <summary>
    /// Affects the Damage of arcane units, the Mana and Mana regen of any unit
    /// </summary>
    Intelligence = 9,
    /// <summary>
    /// Affects Strenght, Agility and intelligence
    /// </summary>
    AllStats = 10,
}

public abstract class StatusEffects : Effect
{
    public AffectedStat AffectedStat;
    public StatusEffects(float value, float duration,AffectedStat affectedStat) : base(value, duration)
    {
        AffectedStat = affectedStat;
    }

    public override void Apply(UnitMovement target)
    {
        target.TESTApplyEffect(this);
    }
}