using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.XR;

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
    public UnitBaseStat Stats { get { return stats; } }
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
    private void OnDisable()
    {
        Destroy(transform.root.gameObject, 0.5f);
    }
    public void Initialize(bool owner, TileType tileType)
    {
        playerOwned = owner;
        StartCoroutine(Co_InitializeUnit(tileType));
    }

    private void OnDestroy()
    {
        IsDead?.Invoke(this);
        //Destroy(gameObject.transform.root.gameObject);
    }

    private void OnValidate()
    {
        UnitStats = new UnitStats(stats.UnitStats.Strenght,
    stats.UnitStats.Agility,
    stats.UnitStats.Intelligence,
    stats.UnitStats.MainStat,
    stats.UnitStats.AttackType,
    stats.UnitStats.AttackRange,
    this);
    }
    private IEnumerator Co_InitializeUnit(TileType tileType)
    {
        UnitAttack unitAttack = gameObject.GetComponent<UnitAttack>();
        UnitMovement unitMovement = gameObject.GetComponent<UnitMovement>();
        UnitSpawnAnimationController controller = new();
        controller.SpawnUnit(gameObject, playerOwned, stats.UnitSpawnAnimationDuration, true);
        yield return new WaitForSeconds(stats.UnitSpawnAnimationDuration);

        unitAttack.enabled = true;
        unitMovement.enabled = true;

        unitAttack.Initialize(this);
        unitMovement.Initialize(this, unitAttack, tileType);
    }
}
