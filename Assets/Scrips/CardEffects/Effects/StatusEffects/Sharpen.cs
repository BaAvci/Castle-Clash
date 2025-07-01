using UnityEngine;

public class Sharpen : StatusEffects
{
    public Sharpen(float duration, AffectedStat affectedStat) : base(25, duration, affectedStat)
    {
    }

    public override string GetDiscription()
    {
        return $"Unit deals {Value}% more damage for {Duration} seconds. It affects {AffectedStat}";
    }
}
