using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class HolyPotion : PlayableCard
{
    public HolyPotion(Actor owner) : base(owner)
    {
    }

    protected override Task InitializeCardDataAsync()
    {
        throw new NotImplementedException();
    }
}
