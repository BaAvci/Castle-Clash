using UnityEngine;

public class Vulnerable : StatusEffect
{
    public Vulnerable(float duration) : base(25, duration, AffectedStat.HealthPoints)
    {
    }

    public override string GetDiscription()
    {
        return $"Unit recieves {Value}% more damage for {Duration} seconds. It affects {AffectedStat}";
    }
}
