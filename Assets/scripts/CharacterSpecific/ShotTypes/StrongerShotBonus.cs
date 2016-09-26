using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.scripts;

public class StrongerShotBonus : UCharacterInteractor
{
    public Weaponry weaponry;

    void Start()
    {
        weaponry = PlayerShootButtonAction.Instance.weapons;
    }

    public override void InteractWith(PlayerScript player)
    {
        var newMultiplier = weaponry.DamageMultiplier * 2;
        if (newMultiplier > weaponry.MaxMultiplier)
            return;

        weaponry.DamageMultiplier = newMultiplier;
    }
}
