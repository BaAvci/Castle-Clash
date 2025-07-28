using NUnit.Framework.Constraints;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;

public class StatusEffect : IEffect
{
    public float Value { get; protected set; }
    public float Duration { get; private set; }
    public float DefaultDuration { get; private set; }
    public SOStatusEffect SOStatusEffect { get; protected set; }

    public StatusEffect(float duration, SOStatusEffect statusEffect)
    {
        this.SOStatusEffect = statusEffect;
        DefaultDuration = duration;
        Duration = DefaultDuration;

#if UNITY_EDITOR
        if (duration <= 0)
        {
            Debug.LogError("Please enter a positive number for duration!");
        }
        if (this.SOStatusEffect.StaticValue == 0)
        {
            Debug.LogError("Please set a StatusValue!");
        }
#endif

        this.Value = this.SOStatusEffect.StaticValue;
    }

    public void ApplyEffect(UnitStats target)
    {
        target.ApplyStatusEffect(this);
    }

    public void UpgradeValues(float duration, float value = 0)
    {
        DefaultDuration = duration;
    }
    public void UpdateStatusDuration(float additionalDuration)
    {
        Duration += additionalDuration;
    }

    public string GetDiscription()
    {
        return "";
    }

    public void ResetValues()
    {
        Duration = DefaultDuration;
    }
}