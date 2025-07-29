using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.ResourceManagement.ResourceProviders.Simulation;
using UnityEngine.VFX;
public abstract class PlayableCard
{
    public event Action<Vector2, TileType> CardWithTileTypePlayed;
    public event Action<Vector2, GameObject> CardWithGameObjectSpawned;
    protected SOCardData cardData;
    [Tooltip("All effects this card can apply to others")]
    private List<IEffect> Effects;
    private int currentLvl;

    public PlayableCard()
    {
        Effects = new List<IEffect>();
        AddEffects();
    }

    private async void AddEffects()
    {
        await InitializeCardDataAsync();
        for (int i = 0; i < cardData.scriptableObjects.Count; i++)
        {
            IEffect effect = null;
            if (cardData.scriptableObjects[i].GetType() == typeof(SOInstantEffects))
            {
                SOInstantEffects sOEffect = cardData.scriptableObjects[i] as SOInstantEffects;
                effect = new InstantEffect(cardData.mainValues[i], sOEffect);
            }
            if (cardData.scriptableObjects[i].GetType() == typeof(SOStatusEffect))
            {
                SOStatusEffect sOEffect = cardData.scriptableObjects[i] as SOStatusEffect;
                effect = new StatusEffect(cardData.mainValues[i], sOEffect);
            }
            if (cardData.scriptableObjects[i].GetType() == typeof(SOOverTimeEffect))
            {
                SOOverTimeEffect sOEffect = cardData.scriptableObjects[i] as SOOverTimeEffect;
                effect = new OverTimeEffect(cardData.mainValues[i], cardData.SecondValues[i], sOEffect);
            }
            //instanceEffect.Add(effect, instantEffectUpgradeValues[i]);
            if (effect == null)
            {
                Debug.LogError("Something wrong happend!");
            }
            Effects.Add(effect);
        }
    }

    public void Upgrade()
    {
        if (currentLvl < cardData.MaxLVL)
        {
            CanUpgrade();
            currentLvl++;
            Debug.Log($"{cardData.Name} has been upgraded and is lvl: {currentLvl}");
        }
    }

    protected abstract Task InitializeCardDataAsync();

    protected virtual void CanUpgrade()
    {
        for (int i = 0; i < Effects.Count; i++)
        {
            float upgradeMainValue = cardData.mainUpgradeValues[i];
            float upgradeSecondValue = cardData.SecondUpgradeValues[i];
            Effects[i].UpgradeValues(upgradeMainValue, upgradeSecondValue);
        }
    }

    public void PlayCard(GameObject target, Vector3 position)
    {
        if (Effects.Count > 0)
        {
            ApplyEffects(target);
        }
        if (cardData.Animation != null)
        {
            SpawnGameObject(cardData.Animation, position);
        }
        if (cardData.Gameobject != null)
        {
            SpawnGameObject(cardData.Gameobject, position);
            CardWithGameObjectSpawned?.Invoke(new Vector2(position.x, position.z), cardData.Gameobject);
        }
        if (cardData.TileType != null)
        {
            CardWithTileTypePlayed?.Invoke(new Vector2(position.x, position.z), cardData.TileType);
        }
    }

    protected virtual void ApplyEffects(GameObject target)
    {
        if (target.TryGetComponent<UnitStats>(out UnitStats unitStats))
        {
            foreach (var effect in Effects)
            {
                effect.ApplyEffect(unitStats);
            }
        }
    }
    protected virtual GameObject SpawnGameObject(GameObject objectToSpawn, Vector3 position)
    {
        return UnityEngine.MonoBehaviour.Instantiate(objectToSpawn, position, Quaternion.identity);
    }
}

