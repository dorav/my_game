using UnityEngine;
using System.Collections;

public class PlayerShieldScript : BasicCharacter
{
    public float shieldRemainingTime;

    public override void Start()
    {
        base.SouldIgnoreCollisions = false;
        if (shieldRemainingTime > 0)
            Destroy(gameObject, shieldRemainingTime);
        base.Start();
    }
}
