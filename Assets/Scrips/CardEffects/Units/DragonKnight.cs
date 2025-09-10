using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AddressableAssets;

public class DragonKnight : PlayableUnitCard
{
    public DragonKnight(Actor owner) : base(owner)
    {
    }

    protected override async Task InitializeCardDataAsync()
    {
        var soCardData = await Addressables.LoadAssetAsync<SOCardData>("Assets/Scrips/CardEffects/Units/DragonKnight.asset").Task;
        CardData = new CardData(soCardData);
    }
}
