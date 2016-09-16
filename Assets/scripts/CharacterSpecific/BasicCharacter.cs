using UnityEngine;
using System.Collections;

public class BasicCharacter : GameCollider
{
    public float Health;
    protected ParticleSystem hitEffectPrefab;
    protected ParticleSystem deathEffectPrefab;

    protected bool SouldIgnoreCollisions = true;

    public virtual void Start()
    {
        hitEffectPrefab = ConstantsDefaultLoader.HitEffectPF;
        deathEffectPrefab = ConstantsDefaultLoader.DeathEffectPF;
    }

    public override void Kill()
    {
        var deathEffect = Instantiate(deathEffectPrefab);
        Vector3 pos = transform.position;
        pos.z = 100;
        deathEffect.transform.position = pos;

        Destroy(deathEffect.gameObject, deathEffect.startLifetime + deathEffect.startLifetime);
        base.Kill();
    }

    public virtual void TakeHitFrom(GameCollider other)
    {
        CreateHitEffect(other.transform);
        Health -= other.Damage;
        if (Health <= 0)
            Kill();
    }

    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        // If this is not done, two collisions will happen.
        // If you still want to collide two triggers (player and
        // an enemy player for example), 
        // set the handle CollideWithOther to true
        if (other.isTrigger && SouldIgnoreCollisions)
            return;

        var dmgDealer = other.GetComponent<GameCollider>();
        TakeHitFrom(dmgDealer);
        RetaliateAgainst(dmgDealer);
    }

    protected static void RetaliateAgainst(GameCollider other)
    {
        other.CollideWithOtherCharacter();
    }

    public override void CollideWithOtherCharacter()
    { 
        Kill();
    }

    protected void CreateHitEffect(Transform hitEffectPosition)
    {
        var explosion = Instantiate(hitEffectPrefab);
        Vector3 pos = hitEffectPosition.position;
        pos.z = 100;
        explosion.transform.position = pos;

        Destroy(explosion.gameObject, explosion.startLifetime + explosion.startLifetime);
    }
}
