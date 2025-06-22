using UnityEngine;

public enum TemperatureType
{
    Normal = 0,
    Warm = 1,
    Cold = 2,
}
public enum MatterState
{
    Solid = 0,
    Plasma = 1,
    Liquid = 2,
    Gas = 3,
}

[CreateAssetMenu(fileName = "ElementalEffect", menuName = "Cellular Automata/Tile Type")]
//TileType
public class TileType : ScriptableObject
{
    public string Name;
    public TemperatureType TemperatureType;
    public MatterState MatterState;
    public GameObject Prefab;
    public int TemperatureValue = 1;
    public int SpreadRange; // How many tiles a TileType can spread until it stops or evaporates
    [SerializeField, Tooltip("Calculated from (Matterstate + Temperature Type) * Temperature Value")] public int Dryness;

    protected void OnValidate()
    {
        int tempTypeValue = (int)TemperatureType;
        Dryness = 1;
        if (TemperatureType == TemperatureType.Warm)
        {
            tempTypeValue += 1;
            Dryness *= -1;
        }
        Dryness *= (tempTypeValue + (int)MatterState) * TemperatureValue;
        if (MatterState == MatterState.Solid)
        {
            SpreadRange = 0;
        }
    }
}

// If TempType same then check MaterState if also same do nothing else change tiletype
// If plasma meets solid, slowly reduce dryness value until plasma dryness value is reached
// Dryness = (Matterstate + TempType) * TemperatureValue
/*
Grass / Fire / Water / Ice / Magma
Normal / Warm / Cold / Cold / Warm
Solid / Plasma / Liquid / Solid / Liquid
0/1/4/2/3
0/-5/5/10/-10
0/-5/20/20/-30
 */

