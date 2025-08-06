using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;
public abstract class PlayableCard
{
    public event Action<Vector2, TileType> CardWithTileTypePlayed;
    public event Action<Vector2, GameObject> CardWithGameObjectSpawned;
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
    public virtual void Play(GameObject target, Vector3 position, List<PlayableCard> playableCards)
    {
        CardPileState = CardState.DiscardPile;
        PlayGameObjectCard(target, position);
    }
    protected void PlayGameObjectCard(GameObject target, Vector3 position)
    {
        if (effects.Count > 0)
        {
            ApplyEffects(target);
        }
        if (CardData.Animation != null)
        {
            SpawnGameObject(CardData.Animation, position);
        }
        if (CardData.Gameobject != null)
        {
            Vector3 corectedspawnPosition = new ((int)position.x,1,(int)position.z);
            SpawnGameObject(CardData.Gameobject, corectedspawnPosition);
            CardWithGameObjectSpawned?.Invoke(new Vector2(position.x, position.z), CardData.Gameobject);
        }
        if (CardData.TileType != null)
        {
            CardWithTileTypePlayed?.Invoke(new Vector2(position.x, position.z), CardData.TileType);
        }
    }
    public void ResetCardState()
    {
        CardPileState = CardState.DrawPile;
        JustCreated = false;
    }
    protected virtual void ApplyEffects(GameObject target)
    {
        if (target.TryGetComponent<UnitStats>(out UnitStats unitStats))
        {
            foreach (var effect in effects)
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

