using UnityEngine;

public abstract class StatusEffect : Effect
{
    public AffectedStat AffectedStat;
    public StatusEffect(float value, float duration,AffectedStat affectedStat) : base(value, duration)
    {
        AffectedStat = affectedStat;
    }
}