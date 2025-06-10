using UnityEngine;

[CreateAssetMenu(fileName = "ElementalEffect", menuName = "Cellular Automata/Elemental Effect")]
//TileType
public class ElementalEffect : ScriptableObject
{
    [Header("Danger Values for affectin other Tiles")]
    public int spreadValue = 1;
    [Tooltip("Gains a spread multiplier against those Elemental Effects")]
    public ElementalEffect[] goodAgainst;

    [Header("Values that affect units")]
    public int Damage;
    public float DamageMultiplier;
}
