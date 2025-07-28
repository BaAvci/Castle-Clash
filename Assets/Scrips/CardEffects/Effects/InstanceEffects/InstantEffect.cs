using UnityEngine;

public class InstantEffect : IEffect
{
    public float Value { get; private set; }
    public float DefaultValue { get; private set; }

    public SOInstantEffects InstantEffects { get; private set; }


    public InstantEffect(float value, SOInstantEffects effects)
    {
        DefaultValue = value;
        this.Value = DefaultValue;
        this.InstantEffects = effects;
    }

    public void ApplyEffect(UnitStats target)
    {
        target.ApplyInstanceModification(this);
    }

    public string GetDiscription()
    {
        string increaseDecrease = InstantEffects.IsPositiv ? "Increases" : "Decreases";
        if (InstantEffects.IsCurrentPercentage)
        {
            return $"{increaseDecrease} the {InstantEffects.AffectedStat} by {Value}% of its current {InstantEffects.AffectedStat}.";
        }
        else if (InstantEffects.IsMaxPercentage)
        {
            return $"{increaseDecrease} the {InstantEffects.AffectedStat}  by  {Value} % of its Max  {InstantEffects.AffectedStat}.";
        }
        else
        {
            return $"{increaseDecrease} the {InstantEffects.AffectedStat} by {Value}.";
        }
    }

    public void UpdateValueOnAffectedUnit(float value)
    {
        this.Value += value;
    }

    public void UpgradeValues(float value, float duration = 0)
    {
        DefaultValue += value;
    }

    public void ResetValues()
    {
        Value = DefaultValue;
    }
}