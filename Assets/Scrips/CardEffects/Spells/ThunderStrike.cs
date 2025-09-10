using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.VFX;

public class ThunderStrike : PlayableSpellCard
{
    public ThunderStrike(Actor owner) : base(owner)
    {
    }
    protected override GameObject SpawnGameObject(GameObject objectToSpawn, Vector3 position)
    {
        var createdGameObject = base.SpawnGameObject(objectToSpawn, position);
        VisualEffect effect = createdGameObject.GetComponentInChildren<VisualEffect>();
        effect.enabled = true;
        WaitForSeconds wait = new WaitForSeconds(1);
        UnitMonoMule.Instance.StartCoroutine(Co_DisplayGameObject(wait, createdGameObject));
        return null;
    }
    private IEnumerator Co_DisplayGameObject(WaitForSeconds wait, GameObject gameObject)
    {
        yield return wait;
        UnitMonoMule.DestroyGameObject(gameObject);
    }

    protected override async Task InitializeCardDataAsync()
    {
        SOCardData soCardData = await Addressables.LoadAssetAsync<SOCardData>("Assets/Scrips/CardEffects/Spells/ThunderStrike.asset").Task;

        CardData = new CardData(soCardData);
    }
}
