using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class ExposingStrike : PlayableCard
{
    #region Animation Data

    #endregion
    public ExposingStrike(Actor owner) : base(owner)
    {
    }

    protected override void CanUpgrade()
    {
        base.CanUpgrade();
    }

    protected override Task InitializeCardDataAsync()
    {
        throw new System.NotImplementedException();
    }
}
