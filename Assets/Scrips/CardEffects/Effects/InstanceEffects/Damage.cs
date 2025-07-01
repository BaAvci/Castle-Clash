using UnityEngine;

public class Damage : InstanceEffect
{
    public Damage(float value) : base(value >= 0 ? value *= -1 : value)
    {
    }

    public override string GetDiscription()
    {
        return $"Deal {Mathf.Abs(Value)} damage to target.";
    }
}
