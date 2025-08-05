using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Unit))]
public class UnitAttack : MonoBehaviour
{
    private Unit owner;
    [SerializeField] private int attackRange;
    public UnitStats? Target { get; private set; }
    [SerializeField] private Unit? unitTarget;
    private float attackTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        owner = GetComponent<Unit>();
        attackRange = owner.UnitStats.AttackRange;
    }

    private void Update()
    {
        if (Target != null)
        {
            attackTimer += Time.deltaTime;
            if (attackTimer >= owner.UnitStats.AttackSpeed)
            {
                if (Target.RecieveDamage(owner.UnitStats.CurrentDamage))
                {
                    SetTarget(null);
                }
                attackTimer = 0;
            }
        }
    }

    public void SetTarget(Unit? unit)
    {
        if (unit == null)
        {
            unitTarget = null;
            this.Target = null;
        }
        else
        {
            unitTarget = unit;
            this.Target = unitTarget.UnitStats;
        }
    }
}
