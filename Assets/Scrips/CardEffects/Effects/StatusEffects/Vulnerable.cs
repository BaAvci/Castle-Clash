using UnityEngine;

public class Vulnerable : StatusEffects
{
    public Vulnerable(float duration, AffectedStat affectedStat) : base(25, duration, affectedStat)
    {
    }

    public override string GetDiscription()
    {
        return $"Unit recieves {Value}% more damage for {Duration} seconds. It affects {AffectedStat}";
    }
}
