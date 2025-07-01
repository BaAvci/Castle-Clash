using UnityEngine;

public class Weaken : StatusEffects
{
    public Weaken(float duration, AffectedStat affectedStat) : base(25, duration, affectedStat)
    {
    }

    public override string GetDiscription()
    {
        return $"Unit deals {Value}% less damage for {Duration} seconds. It affects {AffectedStat}";
    }
}
