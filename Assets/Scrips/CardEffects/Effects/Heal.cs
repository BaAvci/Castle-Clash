using UnityEngine;

public class Heal : InstanceEffect
{
    public Heal(float value) : base(value)
    {
        if (value <= 0)
        {
            Debug.LogError("Heal Value is negative. Change to positive value.");
        }
    }

    public override string GetDiscription()
    {
        return $"Heal target for {value} hp.";
    }
}
