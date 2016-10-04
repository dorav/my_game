using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using Assets.scripts;

public class HealthBonus : UCharacterInteractor
{
    public override void InteractWith(PlayerScript player)
    {
        player.Health += 1;
        player.healthBar.UpdateHealth(player.Health);
    }
}
