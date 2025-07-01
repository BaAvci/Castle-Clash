using UnityEngine;

public class Prayer : OverTimeEffect
{
    public Prayer(float value, float duration) : base(value, duration, AffectedStat.HealthPoints)
    {
    }

    public override string GetDiscription()
    {
        return $"Heals {Value} Healthpoint of a unit for {Duration} seconds!";
    }
}
