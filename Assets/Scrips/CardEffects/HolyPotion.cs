using System.Collections.Generic;
using UnityEngine;

public class HolyPotion : CardData
{
    private float poisonDurationUpgrade = 0.5f;
    private float prayerDurationUpgrade = 1f;
    private float strengtheningDurationUpgrade = 0.25f;
    private float poisonValueUpgrade = 1;
    private float prayerValueUpgrade = 2;
    private float strengtheningValueUpgrade = 0.25f;
    public HolyPotion(GameObject gameObject) : base(3, "Holy Potion", "A holy blessing that grants the unit Healthregen and Strenght gain in exchange for poison", new List<Effect>()
    {
        new Poison(10,5),
        new Prayer(2,20),
        new Strengthening(1,3)
    }, 3, 5, gameObject)
    { }

    protected override void CanUpgrade()
    {
        Effects[0].UpdateValues(poisonValueUpgrade, poisonDurationUpgrade);
        Effects[1].UpdateValues(prayerValueUpgrade, prayerDurationUpgrade);
        Effects[2].UpdateValues(strengtheningValueUpgrade, strengtheningDurationUpgrade);
        foreach (var effect in Effects)
        {
            Debug.Log(effect.GetType());
            Debug.Log(effect.Value);
            Debug.Log(effect.Duration);
        }
    }
}
