using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitBaseStat Stats;
    public static Unit Instance;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
}
