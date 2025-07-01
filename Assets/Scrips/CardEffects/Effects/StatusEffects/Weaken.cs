using UnityEngine;

public class Weaken : StatusEffect
{
    public Weaken(float duration) : base(25, duration, AffectedStat.Damage)
    {
    }

    public override string GetDiscription()
    {
        return $"Unit deals {Value}% less damage for {Duration} seconds. It affects {AffectedStat}";
    }
}
