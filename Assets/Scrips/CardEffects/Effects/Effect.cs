using UnityEngine.UIElements.Experimental;

public abstract class Effect
{
    protected float value;
    protected float duration;
    public string Description;

    public Effect(float value, float duration)
    {
        this.value = value;
        this.duration = duration;
    }

    public abstract void Apply(UnitMovement target);
    public abstract string GetDiscription();
    public void UpdateValues(float value, float duration)
    {
        if (this.GetType() != typeof(InstanceEffect))
        {
            this.duration += duration;
        }
        this.value += value;
    }
    //target.ApplyStatusEffect(value);
    //target.ApplyStatusEffect(duration);
    //target.ApplyStatusEffect(value,duration);

}