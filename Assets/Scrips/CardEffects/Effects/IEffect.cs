using UnityEngine;

public interface IEffect
{
    public void ApplyEffect(UnitStats target);
    public string GetDiscription();
    public void UpgradeValues(float value = 0, float duration = 0);
    public void ResetValues();
}