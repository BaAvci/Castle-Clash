using UnityEngine;

[CreateAssetMenu(fileName = "UnitStat", menuName = "Scriptable Objects/UnitBaseStats")]
public class UnitBaseStat : ScriptableObject
{
    [Range(10, 40)]
    public int Strength;
    [Range(10, 40)]
    public int Agility;
    [Range(10, 40)]
    public int Intelligence;
    public MainStat MainStat;
    public AttackType AttackType;
    [Range(1, 4)]
    public int AttackRange;

    public UnitStats UnitStats;

    /// <summary>
    /// Duration of spawn animation in seconds
    /// </summary>
    public float UnitSpawnAnimationDuration = 4;

    private void OnValidate()
    {
        if (AttackType == AttackType.Ranged && (AttackRange < 2 || AttackRange > 4))
        {
            AttackRange = 2;
        }
        if (AttackType == AttackType.Meele)
        {
            AttackRange = 1;
        }
        UnitStats = new(Strength, Agility, Intelligence, MainStat, AttackType, AttackRange);
    }
}
