using UnityEngine.UIElements.Experimental;

public abstract class Effect
{
    public float Value { get; private set; }
    public float Duration { get; private set; }
    protected string description;

    public Effect(float value, float duration)
    {
        this.Value = value;
        this.Duration = duration;
    }

    public void Apply(UnitMovement target)
    {
        target.TESTApplyEffect(this);
    }
    public abstract string GetDiscription();
    public void UpdateValues(float value, float duration)
    {
        if (this.GetType() != typeof(InstanceEffect))
        {
            this.Duration += duration;
        }
        this.Value += value;
    }
    //target.ApplyStatusEffect(value);
    //target.ApplyStatusEffect(duration);
    //target.ApplyStatusEffect(value,duration);

}