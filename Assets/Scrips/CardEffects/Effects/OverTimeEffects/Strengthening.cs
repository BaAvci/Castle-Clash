using UnityEngine;

public class Strengthening : OverTimeEffect
{
    public Strengthening(float value, float duration) : base(value, duration, AffectedStat.Strenght)
    {
    }

    public override string GetDiscription()
    {
        return $"Unit gains {Value} strengh every second for {Duration} seconds!";
    }
}
