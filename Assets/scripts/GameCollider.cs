using UnityEngine;
using System.Collections;
using System;

public class GameCollider : MonoBehaviour
{
    public float Damage;

    public virtual void Kill()
    {
        Destroy(gameObject);
    }

    public virtual void CollideWithOtherCharacter()
    {
        Kill();
    }
}
