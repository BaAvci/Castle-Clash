using UnityEngine;

[CreateAssetMenu(fileName = "SOStatusEffect", menuName = "Cards/Effects/SOStatusEffect")]
public class SOStatusEffect : ScriptableObject
{
    [Tooltip("Used for Effects like Vunerable or Weaken where the Value should be the same everytime.")]
    public float StaticValue;
    public AffectedStat AffectedStat;
    public bool IsPositiv;
    public bool IsPercentage;
}
