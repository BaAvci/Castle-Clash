using UnityEngine;

public class Poison : OverTimeEffect
{
    public Poison(float value, float duration) : base(value, duration, AffectedStat.HealthPoints)
    {
    }

    public override string GetDiscription()
    {
        return $"Deals {Value} damage to the unit every second for {Duration} seconds";
    }
}
