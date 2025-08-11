using Unity.VisualScripting;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    private Unit owner;
    public UnitStats? Target { get; private set; }
    [SerializeField] private Unit? unitTarget;
    private float attackTimer;

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

    public void Initialize(Unit owner)
    {
        this.owner = owner;
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
