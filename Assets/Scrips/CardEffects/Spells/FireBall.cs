using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class FireBall : PlayableCard
{
    [SerializeField]
    private TileData tileData;
    [SerializeField]
    private int damage = -5;
    [SerializeField]
    private int fireDamage = -5;
    [SerializeField]
    private int fireDuration = 5;

    public FireBall(Actor owner) : base(owner)
    {
    }

    public override void Play(GameObject target, Vector3 position, List<PlayableCard> playableCards)
    {
        PlayGameObjectCard(target, position);
    }

    protected override void CanUpgrade()
    {
        throw new System.NotImplementedException();
    }

    protected override Task InitializeCardDataAsync()
    {
        throw new System.NotImplementedException();
    }
}
