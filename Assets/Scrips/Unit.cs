using UnityEngine;

public class Unit : MonoBehaviour
{
    public UnitBaseStat Stats;
    public static Unit Instance;
    /// <summary>
    /// If True then it's the Player else an Enemy
    /// </summary>
    public bool PlayerOwned => playerOwned;
    [SerializeField] private bool playerOwned;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    public void Initializ(bool owner)
    {
        this.playerOwned = owner;
    }
}
