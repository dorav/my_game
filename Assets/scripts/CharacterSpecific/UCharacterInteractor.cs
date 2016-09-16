using UnityEngine;
using System.Collections;

public abstract class UCharacterInteractor : GameCollider
{
    public abstract void InteractWith(PlayerScript player);
}
