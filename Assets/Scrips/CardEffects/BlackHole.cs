using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class Blackhole : PlayableCard
{
    #region Animation
    private float diameter = 2;
    private float duration = 5;
    private float diameterUpgradeValue = 1;
    private float durationUpgradeValue = 1;
    #endregion

    protected override void CanUpgrade()
    {
        base.CanUpgrade();

        diameter += diameterUpgradeValue;
        duration += durationUpgradeValue;
    }
    protected override GameObject SpawnGameObject(GameObject objectToSpawn, Vector3 position)
    {
        var createdGameObject = base.SpawnGameObject(objectToSpawn, position);
        BlackholeVec blackhole = createdGameObject.GetComponent<BlackholeVec>();
        blackhole.CreateBlackhole(diameter, duration);
        return null;
    }

    protected override async Task InitializeCardDataAsync()
    {
        CardData = await Addressables.LoadAssetAsync<SOCardData>("Assets/Scrips/CardEffects/BlackHole.asset").Task;
    }
}

