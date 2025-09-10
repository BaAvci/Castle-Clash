using Unity.VisualScripting;
using UnityEngine;

public class UnitAttack : MonoBehaviour
{
    private Unit owner;
    public UnitStats Target;
    [SerializeField] private Unit unitTarget;
    private float attackTimer;
    private Animator animator;

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
        animator = gameObject.transform.root.GetComponent<Animator>();
    }
    public void SetTarget(Unit unit)
    {
        if (unit == null)
        {
            unitTarget = null;
            Target = null;
            if (animator != null)
            {
                animator.SetBool("Attacking", false);
            }
        }
        else
        {
            unitTarget = unit;
            Target = unitTarget.UnitStats;
            animator.SetBool("Attacking", true);
            animator.SetFloat("AttackSpeed", owner.UnitStats.AttacksPerSecond);
        }
    }
}
