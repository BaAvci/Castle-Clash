using UnityEngine;

public class Sharpen : StatusEffect
{
    public Sharpen(float duration) : base(25, duration, AffectedStat.Damage)
    {
    }

    public override string GetDiscription()
    {
        return $"Unit deals {Value}% more damage for {Duration} seconds. It affects {AffectedStat}";
    }
}
