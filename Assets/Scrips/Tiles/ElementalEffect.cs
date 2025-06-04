using UnityEngine;

[CreateAssetMenu(fileName = "ElementalEffect", menuName = "Cellular Automata/Elemental Effect")]
public class ElementalEffect : ScriptableObject
{
    /// <summary>
    /// In Seconds
    /// </summary>
    public int DurationTillEffectRemoval;
    public Rule[] ApplyRulesToTile;

    [Header("Danger Values for affectin other Tiles")]
    public int DangerValue;
    public float DangerMultiplier;

    [Header("Save Values for affectin other Tiles")]
    public int SaveValue;
    public float SaveMultiplier;

}
