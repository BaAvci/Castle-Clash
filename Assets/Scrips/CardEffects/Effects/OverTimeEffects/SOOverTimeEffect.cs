using UnityEngine;

[CreateAssetMenu(fileName = "OverTimeEffect", menuName = "Cards/Effects/OverTimeEffect")]
public class SOOverTimeEffect : ScriptableObject
{
    public SOInstantEffects InstanceEffect;
    public float TickRate;
    public bool IsApplyOneTime;
}
