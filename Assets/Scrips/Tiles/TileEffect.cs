using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "TileEffect", menuName = "Tiles/Tile Effect")]
public class TileEffect : ScriptableObject
{
    public Material Material;
    public TileStatus TileStatus;
    public VisualEffect VisualEffect;
}
