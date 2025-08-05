using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitStats UnitStats;
    /// <summary>
    /// If True then it's the Player else an Enemy
    /// </summary>
    public bool PlayerOwned => playerOwned;
    [SerializeField] private UnitBaseStat stats;
    [SerializeField] private bool playerOwned;
    private void Awake()
    {
        UnitStats = new UnitStats(stats.UnitStats.Strenght,
            stats.UnitStats.Agility,
            stats.UnitStats.Intelligence,
            stats.UnitStats.MainStat,
            stats.UnitStats.AttackType,
            stats.UnitStats.AttackRange,
            this);
    }
    public void Initializ(bool owner)
    {
        this.playerOwned = owner;
    }
}
