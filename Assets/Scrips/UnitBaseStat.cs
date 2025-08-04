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
    public UnitStats UnitStats;

    private void OnValidate()
    {
        UnitStats = new(Strenght, Agility, Intelligence, MainStat, AttackType);
    }
}
