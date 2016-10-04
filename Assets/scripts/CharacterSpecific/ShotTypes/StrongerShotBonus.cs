using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.scripts;

public class StrongerShotBonus : UCharacterInteractor
{
    public override void InteractWith(PlayerScript player)
    {
        var newMultiplier = player.weaponry.DamageMultiplier * 2;
        if (newMultiplier > player.weaponry.MaxMultiplier)
            return;

        player.weaponry.DamageMultiplier = newMultiplier;
    }
}
