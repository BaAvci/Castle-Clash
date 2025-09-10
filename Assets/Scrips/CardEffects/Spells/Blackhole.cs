using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;

public class Blackhole : PlayableSpellCard
{
    #region Animation
    private float diameter = 2;
    private float duration = 5;
    private float diameterUpgradeValue = 1;
    private float durationUpgradeValue = 1;
    #endregion

    public Blackhole(Actor owner) : base(owner)
    {
    }

    protected override void CanUpgrade()
    {
        base.CanUpgrade();

        diameter += diameterUpgradeValue;
        duration += durationUpgradeValue;
    }
    protected override GameObject SpawnGameObject(GameObject objectToSpawn, Vector3 position)
    {
        var createdGameObject = base.SpawnGameObject(objectToSpawn, position);
        VisualEffect effect = createdGameObject.GetComponentInChildren<VisualEffect>();
        effect.SetFloat("blackHoleSize", diameter);
        effect.enabled = true;
        WaitForSeconds wait = new WaitForSeconds(duration);
        UnitMonoMule.Instance.StartCoroutine(Co_DisplayGameObject(wait, createdGameObject));
        return null;
    }
    private IEnumerator Co_DisplayGameObject(WaitForSeconds wait,GameObject gameObject)
    {
        yield return wait;
        UnitMonoMule.DestroyGameObject(gameObject);
    }

    protected override async Task InitializeCardDataAsync()
    {
        SOCardData soCardData = await Addressables.LoadAssetAsync<SOCardData>("Assets/Scrips/CardEffects/Spells/Blackhole.asset").Task;

        CardData = new CardData(soCardData);
    }

}

