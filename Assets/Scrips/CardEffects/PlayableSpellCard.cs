using UnityEngine;
public enum SpellCardType
{
    Damage,
    Buff,
}
    
public abstract class PlayableSpellCard : PlayableCard
{
    [SerializeField] SpellCardType cardType;
    protected PlayableSpellCard(Actor owner) : base(owner)
    {
    }
}
