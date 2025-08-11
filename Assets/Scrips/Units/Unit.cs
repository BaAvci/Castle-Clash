using System;
using UnityEngine;

[RequireComponent(typeof(UnitMovement), typeof(UnitAttack))]
public class Unit : MonoBehaviour
{
    public event Action<Unit> IsDead;
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
    public void Initialize(bool owner, TileType tileType)
    {
        playerOwned = owner;
        UnitAttack unitAttack = gameObject.GetComponent<UnitAttack>();
        unitAttack.Initialize(this);
        gameObject.GetComponent<UnitMovement>().Initialize(this, unitAttack, tileType);
    }
    private void Update()
    {
        if (!gameObject.activeSelf)
        {
            Destroy(this, 0.5f);
        }
    }
    private void OnDestroy()
    {
        IsDead?.Invoke(this);
    }
}
