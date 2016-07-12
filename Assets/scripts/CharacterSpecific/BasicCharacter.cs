using UnityEngine;
using System.Collections;

public class BasicCharacter : GameCollider
{
    public float Health;
    protected ParticleSystem hitEffectPrefab;
    protected ParticleSystem deathEffectPrefab;

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

    public virtual void TakeHit(GameCollider other)
    {
        Health -= other.Damage;
        if (Health <= 0)
            Kill();
    }

    // Other is configured so it can either be another character or a bullet
    protected virtual void OnTriggerEnter2D(Collider2D other)
    {
        CreateHitEffect(other);
        var dmgDealer = other.GetComponent<GameCollider>();
        TakeHit(dmgDealer);
        OtherTakeHit(dmgDealer);
    }

    private static void OtherTakeHit(GameCollider other)
    {
        // If other is the player, this = other character.
        // handling will be done on the player's event handler
        if (other.gameObject.tag == "Player")
            return;

        other.CollideWithOtherCharacter();
    }

    public override void CollideWithOtherCharacter()
    { 
        Kill();
    }

    private void CreateHitEffect(Collider2D other)
    {
        var explosion = Instantiate(hitEffectPrefab);
        Vector3 pos = other.transform.position;
        pos.z = 100;
        explosion.transform.position = pos;

        Destroy(explosion.gameObject, explosion.startLifetime + explosion.startLifetime);
    }
}
