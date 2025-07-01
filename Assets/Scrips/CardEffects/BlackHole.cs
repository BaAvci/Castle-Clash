using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackHole : CardData
{
    public float DamageValue;
    private float diameter = 2;
    private float duration = 5;
    private float diameterUpgradeValue = 1;
    private float durationUpgradeValue = 1;
    private float damageUpgradeValue = 5;

    public BlackHole(GameObject gameObject) : base(1, "Blackhole", "Creates a blackhole that pulls units to the center and deals damage", new List<Effect>()
    {
        new Damage(-5),
        new Heal(5),
    }, 3, 3, gameObject)
    {
    }

    // TODO: Move cardCreation to ScriptableObject
    protected override void CanUpgrade()
    {
        diameter += diameterUpgradeValue;
        duration += durationUpgradeValue;
        Effects[0].UpdateValues(damageUpgradeValue, 0);
        Effects[1].UpdateValues(damageUpgradeValue, 0);
    }

    public override void Spawn(GameObject target, GameObject spawnedObject)
    {
        base.Spawn(target, spawnedObject);
        Blackhole blackhole = spawnedObject.GetComponent<Blackhole>();
        blackhole.CreateBlackhole(diameter, duration);
    }
}
