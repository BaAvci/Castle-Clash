using UnityEngine;

[CreateAssetMenu(fileName = "InstantEffects", menuName = "Cards/Effects/InstantEffects")]
public class SOInstantEffects : ScriptableObject
{
    public AffectedStat AffectedStat;
    public bool IsPositiv;
    public bool IsCurrentPercentage;
    public bool IsMaxPercentage;
}
