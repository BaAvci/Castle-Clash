using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
public abstract class PlayableCard
{
    public event Action<TileType, Vector2> CardWithTileTypePlayed;
    public event Action<Vector3, GameObject> CardWithGameObjectSpawned;
    public CardState CardPileState;
    public CardData CardData;
    [Tooltip("All effects this card can apply to others")]
    private List<IEffect> effects;
    private int currentLvl;
    public bool JustCreated;
    protected bool isPlayerOwned;

    public PlayableCard(Actor owner)
    {
        if (owner.gameObject.CompareTag("Player"))
        {
            isPlayerOwned = true;
        }
        else
        {
            isPlayerOwned = false;
        }
        effects = new List<IEffect>();
        AddEffects();
    }

    private async void AddEffects()
    {
        await InitializeCardDataAsync();

        for (int i = 0; i < CardData.ScriptableObjects.Count; i++)
        {
            IEffect effect = null;
            if (CardData.ScriptableObjects[i].GetType() == typeof(SOInstantEffects))
            {
                SOInstantEffects sOEffect = CardData.ScriptableObjects[i] as SOInstantEffects;
                effect = new InstantEffect(CardData.MainValues[i], sOEffect);
            }
            if (CardData.ScriptableObjects[i].GetType() == typeof(SOStatusEffect))
            {
                SOStatusEffect sOEffect = CardData.ScriptableObjects[i] as SOStatusEffect;
                effect = new StatusEffect(CardData.MainValues[i], sOEffect);
            }
            if (CardData.ScriptableObjects[i].GetType() == typeof(SOOverTimeEffect))
            {
                SOOverTimeEffect sOEffect = CardData.ScriptableObjects[i] as SOOverTimeEffect;
                effect = new OverTimeEffect(CardData.MainValues[i], CardData.SecondValues[i], sOEffect);
            }
            //instanceEffect.Add(effect, instantEffectUpgradeValues[i]);
            if (effect == null)
            {
                Debug.Log(isPlayerOwned);
                Debug.Log(CardData.Name);
                Debug.LogError("Something wrong happend!");
            }
            effects.Add(effect);
        }
    }

    public void Upgrade()
    {
        if (currentLvl < CardData.MaxLVL)
        {
            CanUpgrade();
            currentLvl++;
            Debug.Log($"{CardData.Name} has been upgraded and is lvl: {currentLvl}");
        }
    }

    protected abstract Task InitializeCardDataAsync();

    protected virtual void CanUpgrade()
    {
        for (int i = 0; i < effects.Count; i++)
        {
            float upgradeMainValue = CardData.MainUpgradeValues[i];
            float upgradeSecondValue = CardData.SecondUpgradeValues[i];
            effects[i].UpgradeValues(upgradeMainValue, upgradeSecondValue);
        }
    }
    public virtual void Draw(List<PlayableCard> playableCards)
    {
        CardPileState = CardState.HandPile;
    }
    public virtual void Shuffle(List<PlayableCard> playableCards)
    {
        CardPileState = CardState.DrawPile;
    }
    public virtual void Discard(List<PlayableCard> playableCards)
    {
        CardPileState = CardState.DiscardPile;
    }
    public virtual void Create(List<PlayableCard> playableCards)
    {
        CardPileState = CardState.DrawPile;
    }

    public virtual void Play(List<Unit> targets, Vector3 position, List<PlayableCard> playableCards)
    {
        CardPileState = CardState.DiscardPile;

        PlayGameObjectCard(targets, position);
    }
    protected void PlayGameObjectCard(List<Unit> targets, Vector3 position)
    {
        if (effects.Count > 0)
        {
            ApplyEffects(targets);
        }
        if (CardData.Animation != null)
        {
            SpawnGameObject(CardData.Animation, position);
        }
        if (CardData.Gameobject != null)
        {
            GameObject spawnedObject = SpawnGameObject(CardData.Gameobject, position);
            spawnedObject.GetComponent<Unit>().Initialize(isPlayerOwned, CardData.TileType);
            CardWithGameObjectSpawned?.Invoke(position, spawnedObject);
        }
        if (CardData.TileType != null)
        {
            CardWithTileTypePlayed?.Invoke(CardData.TileType, new Vector2(position.x, position.z));
        }
    }
    public void ResetCardState()
    {
        CardPileState = CardState.DrawPile;
        JustCreated = false;
    }
    protected virtual void ApplyEffects(List<Unit> targets)
    {
        for (int i = 0; i < targets.Count; i++)
        {
            for (int j = 0; j < effects.Count; j++)
            {
                effects[j].ApplyEffect(targets[i].UnitStats);
            }
        }
    }

    protected virtual GameObject SpawnGameObject(GameObject objectToSpawn, Vector3 position)
    {
        return UnityEngine.MonoBehaviour.Instantiate(objectToSpawn, position, Quaternion.identity);
    }
}

