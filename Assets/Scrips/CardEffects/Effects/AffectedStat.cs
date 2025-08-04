using System.Runtime.CompilerServices;

public enum AffectedStat
{
    /// <summary>
    /// Affects Strenght, Agility and intelligence
    /// </summary>
    AllStats = 0,
    /// <summary>
    /// How much damage something deals to something else
    /// </summary>
    Damage = 1,
    /// <summary>
    /// Affects all kinds of basestats
    /// </summary>
    BaseStats = 10,
    /// <summary>
    /// Affects all kinds of regeneration
    /// </summary>
    Regen = 20,
    /// <summary>
    /// Affects all kinds of regeneration
    /// </summary>
    Resistance = 30,
    /// <summary>
    /// Affects all kinds of amplification
    /// </summary>
    Amplification = 40,
    #region Strenght
    /// <summary>
    /// Affects the Damage of tank units, the HP and HP regen of any unit
    /// </summary>
    Strenght = 100,
    /// <summary>
    /// How much HP a unit has
    /// </summary>
    HealthPoints = 110,
    /// <summary>
    /// How much HP a unit is regening per second
    /// </summary>
    HealthRegen = 120,
    /// <summary>
    /// How much CrowdControl (Forced movement, stuns) resistance a Unit has
    /// </summary>
    CrowdControlResistance = 130,
    /// <summary>
    /// How much % Physical Damage a Unit resists
    /// </summary>
    PhysicalResistance = 131,
    /// <summary>
    /// The amount of additional healing a unit gains
    /// </summary>
    HealingAmplification = 140,
    #endregion
    #region Agility
    /// <summary>
    /// Affects the Damage of swift units, the Armor and Movement/Attack speed of any unit
    /// </summary>
    Agility = 200,
    /// <summary>
    /// How much Flat Physical Damage is blocked
    /// </summary>
    Armor = 210,
    /// <summary>
    /// Affect the speed of the unit
    /// </summary>
    Speed = 211,
    /// <summary>
    /// The chance the unit has to deal damage with a critcal hit
    /// </summary>
    CritChance = 212,
    /// <summary>
    /// The chance a unit has to evade an attack
    /// </summary>
    EvadeChance = 213,
    /// <summary>
    /// How likely it is for a unit to resist a Critical hit.
    /// </summary>
    CritResistance = 230,
    #endregion
    #region Intelligence
    /// <summary>
    /// Affects the Damage of arcane units, the Mana and Mana regen of any unit
    /// </summary>
    Intelligence = 300,
    /// <summary>
    /// How much Mana a unit has
    /// </summary>
    Mana = 310,
    /// <summary>
    /// How much Mana a unit is regening per second
    /// </summary>
    ManaRegen = 320,
    /// <summary>
    /// How much negativ Status (Weaken, Vulnerable) resistance a Unit has.
    /// </summary>
    StatusResistance = 330,
    /// <summary>
    /// How much % magic damage a unit blocks
    /// </summary>
    MagicalResistance = 331,
    /// <summary>
    /// The amount of additional damage a unit deals with unit specific spell.
    /// </summary>
    SpellAmplification = 340,
    #endregion

}