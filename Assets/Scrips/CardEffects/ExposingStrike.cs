using System.Collections.Generic;
using UnityEngine;

public class ExposingStrike : CardData
{
    private float weakenUpgradeValue = 1.5f;
    private float vulnerableUpgradeValue = 1.5f;
    private float sharpenUpgradeValue = 1.5f;
    public ExposingStrike(GameObject gameObject) : base(2, "Exposing Strike", "Exposes the target unit leaving it in a weakend and vulnerable state", new List<Effect>() { new Vulnerable(5, AffectedStat.HealthPoints), new Weaken(3, AffectedStat.Damage), new Sharpen(10, AffectedStat.Damage) }, 3, 6, gameObject)
    {
    }

    protected override void CanUpgrade()
    {
        Effects[0].UpdateValues(0, vulnerableUpgradeValue);
        Effects[1].UpdateValues(0, weakenUpgradeValue);
        Effects[2].UpdateValues(0, sharpenUpgradeValue);
    }
    public override void Spawn(GameObject target, GameObject spawnedObject)
    {
        base.Spawn(target, spawnedObject);
    }
}
