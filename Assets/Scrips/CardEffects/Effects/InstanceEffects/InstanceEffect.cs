using UnityEngine;

public abstract class InstanceEffect : Effect
{
    /// <summary>
    /// This value is the damage, healing or simmilar cases value
    /// </summary>
    protected InstanceEffect(float value) : base(value, 0) { }

    public override void Apply(UnitMovement target)
    {
        target.TESTApplyEffect(this);
    }
}