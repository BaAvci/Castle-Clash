using UnityEngine;

public abstract class OverTimeEffect : Effect
{
    protected OverTimeEffect(float value, float duration, AffectedStat affectedStat) : base(value, duration)
    {
    }
}
