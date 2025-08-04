using System.Runtime.InteropServices;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class OverTimeEffect : IEffect
{
    public InstantEffect InstanceEffect { get; private set; }
    public float TickRate { get; private set; }
    public float Duration { get; private set; }
    public bool IsApplyOneTime { get; private set; }

    private float defaultDuration;

    public OverTimeEffect(float value, float duration, SOOverTimeEffect overTimeEffect)
    {
        defaultDuration = duration;
        Duration = defaultDuration;
        IsApplyOneTime = overTimeEffect.IsApplyOneTime;
        TickRate = overTimeEffect.TickRate;
        InstanceEffect = new InstantEffect(value, overTimeEffect.InstanceEffect);
    }

    public void ApplyEffect(UnitStats target)
    {
        target.ApplyOverTimeEffect(this);
    }

    /// <summary>
    /// Function is only called when OverTimeEffect has already been applied once.
    /// </summary>
    /// <param name="value"></param>
    /// <param name="duration"></param>
    public void UpgradeValues(float value, float duration)
    {
        if (!IsApplyOneTime)
        {
            InstanceEffect.UpgradeValues(value);
        }
        Duration = duration;
    }

    public void UpdateValuesOnAffectedUnit(float value, float duration)
    {
        InstanceEffect.UpdateValueOnAffectedUnit(value + InstanceEffect.Value);
        Duration += duration;
    }
    public void ApplyInstanceEffect(UnitStats target)
    {
        InstanceEffect.ApplyEffect(target);
    }
    public string GetDiscription()
    {
        return "Not implemented yet!";
    }

    public void ResetValues()
    {
        Duration = defaultDuration;
        InstanceEffect.ResetValues();
    }
}
