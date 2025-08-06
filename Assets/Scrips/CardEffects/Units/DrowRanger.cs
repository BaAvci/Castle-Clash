using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DrowRanger : PlayableCard
{
    public DrowRanger(Actor owner) : base(owner)
    {
    }

    protected override async Task InitializeCardDataAsync()
    {
        var soCardData = await Addressables.LoadAssetAsync<SOCardData>("Assets/Scrips/CardEffects/Units/DrowRanger.asset").Task;
        CardData = new CardData(soCardData);
        CardData.Gameobject.GetComponent<Unit>().Initializ(isPlayerOwned);
    }
}
