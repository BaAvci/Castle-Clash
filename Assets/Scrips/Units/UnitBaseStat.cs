using UnityEngine;

[CreateAssetMenu(fileName = "UnitStat", menuName = "Scriptable Objects/UnitBaseStats")]
public class UnitBaseStat : ScriptableObject
{
    [Range(10, 40)]
    public float Strenght;
    [Range(10, 40)]
    public float Agility;
    [Range(10, 40)]
    public float Intelligence;
    public MainStat MainStat;
    public AttackType AttackType;
    [Range(1, 4)]
    public int AttackRange;

    public UnitStats UnitStats;

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
        UnitStats = new(Strenght, Agility, Intelligence, MainStat, AttackType, AttackRange);
    }
}
